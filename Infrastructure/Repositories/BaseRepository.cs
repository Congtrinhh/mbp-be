using Application.Interfaces;
using AutoMapper;
using Dapper;
using Domain.Attributes;
using Domain.Entities;
using Domain.Entities.Paging;
using Domain.Enums;
using Domain.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// base repository class for CRUD operations. (lớp repository cơ sở cho các hoạt động CRUD)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T, TDto> : IBaseRepository<T, TDto> where T : BaseEntity where TDto : BaseEntity
    {
        protected readonly IDbConnection DbConnection;
        protected readonly IMapper Mapper;

        public BaseRepository(IDbConnection dbConnection, IMapper mapper)
        {
            DbConnection = dbConnection;
            Mapper = mapper;
            UseSnakeCaseMapping<T>();
            UseSnakeCaseMapping<TDto>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            string entityName = StringExtension.ToSnakeCase(typeof(T).Name);
            var sql = $"SELECT * FROM {entityName}";
            return await DbConnection.QueryAsync<T>(sql);
        }

        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            string entityName = StringExtension.ToSnakeCase(typeof(T).Name);

            var sql = $"SELECT * FROM {entityName} WHERE Id = @Id";
            var entity = await DbConnection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
            var dto = Mapper.Map<TDto>(entity);
            return dto;
        }

        private void EnsureConnectionOpen()
        {
            if (DbConnection.State != ConnectionState.Open)
            {
                DbConnection.Open();
            }
        }

        public virtual async Task<int> AddAsync(TDto entity)
        {
            EnsureConnectionOpen();
            using (var transaction = DbConnection.BeginTransaction())
            {
                try
                {
                    entity.EntityState = EntityState.Add;

                    var sql = GenerateInsertSql(entity);
                    var result = await DbConnection.ExecuteScalarAsync<int>(sql, entity, transaction);

                    entity.GetType().GetProperty("Id")!.SetValue(entity, result);

                    await HandleNestedEntitiesAsync(entity, transaction);
                    transaction.Commit();
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public virtual async Task<int> UpdateAsync(TDto entity)
        {
            EnsureConnectionOpen();
            using (var transaction = DbConnection.BeginTransaction())
            {
                try
                {
                    if (entity.Id == 0)
                    {
                        throw new Exception("Id is required for update operation");
                    }

                    entity.EntityState = EntityState.Update;

                    var sql = GenerateUpdateSql(entity);

                    int affectedRows = await DbConnection.ExecuteAsync(sql, entity, transaction);

                    await HandleNestedEntitiesAsync(entity, transaction);

                    transaction.Commit();
                    return affectedRows;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            EnsureConnectionOpen();
            var sql = $"DELETE FROM {typeof(T).Name.ToSnakeCase()} WHERE id = @Id";
            return await DbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public virtual async Task<PagedResponse<TDto>> GetPagedAsync(PagedRequest pagedRequest)
        {
            EnsureConnectionOpen();

            if (pagedRequest is PagedRequest basePagedRequest)
            {
                var whereClause = GenerateWhereClause(basePagedRequest);
                var sortClause = string.IsNullOrEmpty(basePagedRequest.Sort) ? "created_at DESC" : basePagedRequest.Sort;

                var sql = $@"
                            SELECT * FROM {typeof(T).Name.ToSnakeCase()}
                            {whereClause}
                            ORDER BY {sortClause}";

                if (basePagedRequest.PageSize != -1)
                {
                    sql += " LIMIT @PageSize OFFSET @Offset";
                }

                sql += $@";
                 SELECT COUNT(*) FROM {typeof(T).Name.ToSnakeCase()}
                 {whereClause};";

                var parameters = new DynamicParameters();
                if (basePagedRequest.PageSize != -1)
                {
                    parameters.Add("Offset", basePagedRequest.PageIndex * basePagedRequest.PageSize);
                    parameters.Add("PageSize", basePagedRequest.PageSize);
                }

                sql = AppendSqlGetDetails(basePagedRequest, whereClause, sortClause, sql);

                AddFilterParameters(parameters, pagedRequest);

                var multi = await DbConnection.QueryMultipleAsync(sql, parameters);
                var items = multi.Read<TDto>().ToList();
                var totalCount = multi.ReadSingle<int>();

                BindDetails(basePagedRequest, multi, items);

                return new PagedResponse<TDto>(items, totalCount, basePagedRequest.PageSize, basePagedRequest.PageIndex);
            }

            return new PagedResponse<TDto>(new List<TDto>(), 0, 10, 1);
        }

        /// <summary>
        /// nếu lấy thêm detail thì bind vào master
        /// </summary>
        /// <param name="basePagedRequest"></param>
        /// <param name="multi"></param>
        /// <param name="items"></param>
        protected virtual void BindDetails(PagedRequest basePagedRequest, SqlMapper.GridReader multi, List<TDto> items)
        {
        }

        /// <summary>
        /// gán thêm câu sql select các detail
        /// </summary>
        /// <param name="basePagedRequest"></param>
        /// <param name="whereClause"></param>
        /// <param name="sortClause"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected virtual string AppendSqlGetDetails(PagedRequest basePagedRequest, string whereClause, string sortClause, string sql)
        {
            return sql;
        }

        protected virtual string GenerateWhereClause(PagedRequest pagedRequest)
        {
            var sb = new StringBuilder("WHERE 1=1 ");
            return sb.ToString();
        }

        protected virtual void AddFilterParameters(DynamicParameters parameters, PagedRequest pagedRequest)
        {
        }

        public static void UseSnakeCaseMapping<T>()
        {
            var type = typeof(T);
            var map = new CustomPropertyTypeMap(
                type,
                (type, columnName) => type.GetProperties().FirstOrDefault(prop => prop.Name.ToSnakeCase() == columnName)
            );

            SqlMapper.SetTypeMap(type, map);
        }

        private string GenerateInsertSql(TDto entity)
        {
            var properties = typeof(TDto).GetProperties().Where(p => p.GetCustomAttribute<OneToManyAttribute>() == null && p.GetCustomAttribute<ManyToManyAttribute>() == null && p.GetCustomAttribute<NotMappedAttribute>() == null &&
            p.GetCustomAttribute<OneToOneAttribute>() == null && p.Name != "Id" && p.HasValue(entity));

            var columns = string.Join(", ", properties.Select(p => p.Name.ToSnakeCase()));
            var values = string.Join(", ", properties.Select(p => "@" + p.Name));
            string entityName = GetEntityDbName(entity);
            string sql = $"INSERT INTO {entityName} ({columns}) VALUES ({values});";

            sql += "SELECT LAST_INSERT_ID();";

            return sql;
        }

        /// <summary>
        /// câu sql insert cho detail one to many
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string GenerateInsertSql(BaseEntity entity)
        {
            var entityName = GetEntityDbName(entity);
            var properties = entity.GetType().GetProperties().Where(p => p.GetCustomAttribute<OneToManyAttribute>() == null && p.GetCustomAttribute<ManyToManyAttribute>() == null && p.GetCustomAttribute<NotMappedAttribute>() == null &&
            p.GetCustomAttribute<OneToOneAttribute>() == null && p.Name != "Id" && p.HasValue(entity));

            var columns = string.Join(", ", properties.Select(p => p.Name.ToSnakeCase()));
            var values = string.Join(", ", properties.Select(p => "@" + p.Name));

            string sql = $"INSERT INTO {entityName} ({columns}) VALUES ({values});";
            sql += "SELECT LAST_INSERT_ID();";

            return sql;
        }

        /// <summary>
        /// câu sql update cho detail one to many
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string GenerateUpdateSql(BaseEntity entity)
        {
            var entityName = GetEntityDbName(entity);
            var properties = entity.GetType().GetProperties().Where(p => p.GetCustomAttribute<OneToManyAttribute>() == null && p.GetCustomAttribute<ManyToManyAttribute>() == null && p.GetCustomAttribute<NotMappedAttribute>() == null &&
            p.GetCustomAttribute<OneToOneAttribute>() == null && p.Name != "Id" && p.HasValue(entity));
            var setClause = string.Join(", ", properties.Select(p => $"{p.Name.ToSnakeCase()} = @{p.Name}"));

            if (properties.Count() > 0)
            {
                setClause += ",";
            }
            setClause += " modified_at = NOW()";

            return $"UPDATE {entityName} SET {setClause} WHERE id = @Id;";
        }

        private string GenerateUpdateSql(TDto entity)
        {
            var properties = typeof(TDto).GetProperties().Where(p => p.GetCustomAttribute<OneToManyAttribute>() == null && p.GetCustomAttribute<ManyToManyAttribute>() == null && p.GetCustomAttribute<NotMappedAttribute>() == null &&
            p.GetCustomAttribute<OneToOneAttribute>() == null && p.Name != "Id" && p.HasValue(entity));
            var setClause = string.Join(", ", properties.Select(p => $"{p.Name.ToSnakeCase()} = @{p.Name}"));
            string entityName = GetEntityDbName(entity);

            if (properties.Count() > 0)
            {
                setClause += ",";
            }
            setClause += " modified_at = NOW()";

            return $"UPDATE {entityName} SET {setClause} WHERE id = @Id;";
        }

        private async Task HandleNestedEntitiesAsync(TDto entity, IDbTransaction transaction)
        {
            var nestedEntityInfos = typeof(TDto).GetProperties()
                .Where(p =>
                {
                    return typeof(IEnumerable<object>).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string) && (p.GetCustomAttribute<ChildTableAttribute>() != null || p.GetCustomAttribute<ManyToManyAttribute>() != null);
                })
                .Select(p =>
                {
                    return new { Property = p, Value = p.GetValue(entity) };
                }).ToList();

            if (nestedEntityInfos == null || nestedEntityInfos.Count == 0)
            {
                return;
            }

            foreach (var nestedEntityInfo in nestedEntityInfos)
            {
                if (nestedEntityInfo.Value == null) continue;

                var tmpNestedEntities = nestedEntityInfo.Value as IEnumerable<object>;
                if (tmpNestedEntities == null) continue;

                List<BaseEntity> nestedEntities = tmpNestedEntities.OfType<BaseEntity>().ToList();

                if (nestedEntities == null || nestedEntities.Count == 0)
                {
                    continue;
                }

                int parentId = entity.Id;
                EntityState parentEntityState = entity.EntityState;

                if (parentEntityState == EntityState.Add)
                {
                    nestedEntities.ForEach(item => item.EntityState = EntityState.Add);
                }

                foreach (var item in nestedEntities)
                {
                    EntityState entityState = item.EntityState;
                    if (entityState == EntityState.None) continue;
                    bool isIdEmpty = item.Id == 0;
                    if (entityState == EntityState.Update && isIdEmpty) continue;
                    if (entityState == EntityState.Delete && isIdEmpty) continue;

                    var manyToManyAttr = nestedEntityInfo.Property.GetCustomAttribute<ManyToManyAttribute>();
                    if (manyToManyAttr != null)
                    {
                        // Handle many-to-many relationship
                        string joinTable = manyToManyAttr.JoinTable;
                        string joinColumn = manyToManyAttr.JoinColumn;
                        string inverseJoinColumn = manyToManyAttr.InverseJoinColumn;

                        if (entityState == EntityState.Add)
                        {
                            string sql = $"INSERT INTO {joinTable} ({joinColumn}, {inverseJoinColumn}) VALUES (@ParentId, @ChildId)";
                            await DbConnection.ExecuteAsync(sql, new { ParentId = parentId, ChildId = item.Id }, transaction);
                        }
                        else if (entityState == EntityState.Delete)
                        {
                            string sql = $"DELETE FROM {joinTable} WHERE {joinColumn} = @ParentId AND {inverseJoinColumn} = @ChildId";
                            await DbConnection.ExecuteAsync(sql, new { ParentId = parentId, ChildId = item.Id }, transaction);
                        }
                    }
                    else
                    {
                        // Handle one-to-many relationship
                        var childParentIdProperty = item.GetType().GetProperty($"{typeof(T).Name}Id");
                        if (childParentIdProperty != null)
                        {
                            childParentIdProperty.SetValue(item, parentId);
                        }

                        string sql = entityState switch
                        {
                            EntityState.Add => GenerateInsertSql(item),
                            EntityState.Update => GenerateUpdateSql(item),
                            EntityState.Delete => GenerateDeleteSql(item),
                            _ => null
                        };

                        if (string.IsNullOrEmpty(sql)) continue;

                        await DbConnection.ExecuteAsync(sql, item, transaction);
                    }
                }
            }
        }

        private string GenerateDeleteSql(BaseEntity entity)
        {
            string name = GetEntityDbName(entity);
            return $"DELETE FROM {name} WHERE id = @Id";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string GetEntityDbName(BaseEntity entity)
        {
            TableAttribute attr = entity.GetType().GetCustomAttribute<TableAttribute>();
            if (attr != null && !string.IsNullOrWhiteSpace(attr.Name))
            {
                return attr.Name;
            }

            return entity.GetType().Name.ToSnakeCase();
        }

        public virtual async Task<TDto> FindByFieldAsync(string fieldName, object value)
        {
            var entityName = typeof(T).Name.ToSnakeCase();
            var sql = $"SELECT * FROM {entityName} WHERE {fieldName.ToSnakeCase()} = @Value";
            return await DbConnection.QuerySingleOrDefaultAsync<TDto>(sql, new { Value = value });
        }
    }
}
