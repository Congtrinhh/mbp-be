using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Paging;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EFCoreBaseRepository<T, TDto> : IBaseRepository<T, TDto> where T : BaseEntity where TDto : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        public EFCoreBaseRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual async Task<int> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<T>(dto);

            // Handle many-to-many navigation properties generically
            foreach (var nav in typeof(T).GetProperties().Where(p =>
                typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) &&
                p.PropertyType != typeof(string)))
            {
                var dtoNav = typeof(TDto).GetProperty(nav.Name);
                if (dtoNav == null) continue;
                var relatedDtos = dtoNav.GetValue(dto) as System.Collections.IEnumerable;
                if (relatedDtos == null) continue;

                var entityType = nav.PropertyType.GenericTypeArguments.FirstOrDefault();
                if (entityType == null) continue;

                var dbSetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes).MakeGenericMethod(entityType);
                var dbSet = dbSetMethod.Invoke(_context, null);
                var findMethod = dbSet.GetType().GetMethod("Find", new[] { typeof(object[]) });
                var relatedEntities = new List<object>();
                foreach (var relatedDto in relatedDtos)
                {
                    var idProp = relatedDto.GetType().GetProperty("Id");
                    if (idProp == null) continue;
                    var id = idProp.GetValue(relatedDto);
                    var tracked = findMethod.Invoke(dbSet, new object[] { new object[] { id } });
                    if (tracked != null)
                        relatedEntities.Add(tracked);
                }
                var entityNav = nav.GetValue(entity) as System.Collections.IList;
                if (entityNav != null)
                {
                    foreach (var rel in relatedEntities)
                        entityNav.Add(rel);
                }
            }

            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return 0;
            }
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual Task<PagedResponse<TDto>> GetPagedAsync(PagedRequest pagedRequest)
        {
            throw new System.NotImplementedException();
        }

        public virtual async Task<int> UpdateAsync(TDto dto)
        {
            var entity = await _context.Set<T>()
                .AsTracking()
                .FirstOrDefaultAsync(e => e.Id == dto.Id);

            if (entity == null)
                throw new InvalidOperationException("Entity not found");

            _mapper.Map(dto, entity);

            // Handle many-to-many navigation properties generically
            foreach (var nav in typeof(T).GetProperties().Where(p =>
                typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) &&
                p.PropertyType != typeof(string)))
            {
                var dtoNav = typeof(TDto).GetProperty(nav.Name);
                if (dtoNav == null) continue;
                var relatedDtos = dtoNav.GetValue(dto) as System.Collections.IEnumerable;
                if (relatedDtos == null) continue;

                var entityType = nav.PropertyType.GenericTypeArguments.FirstOrDefault();
                if (entityType == null) continue;

                var dbSetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes).MakeGenericMethod(entityType);
                var dbSet = dbSetMethod.Invoke(_context, null);
                var findMethod = dbSet.GetType().GetMethod("Find", new[] { typeof(object[]) });

                var entityNav = nav.GetValue(entity) as System.Collections.IList;
                if (entityNav == null) continue;

                // Remove entities not in DTO
                var dtoIds = new HashSet<object>(
                    relatedDtos.Cast<object>()
                        .Where(d => d != null)
                        .Select(d => d.GetType().GetProperty("Id")?.GetValue(d))
                        .Where(id => id != null)
                );
                for (int i = entityNav.Count - 1; i >= 0; i--)
                {
                    var navEntity = entityNav[i];
                    var navId = navEntity.GetType().GetProperty("Id")?.GetValue(navEntity);
                    if (!dtoIds.Contains(navId))
                        entityNav.RemoveAt(i);
                }

                // Add new entities from DTO (only tracked, not already present)
                foreach (var relatedDto in relatedDtos)
                {
                    if (relatedDto == null) continue;
                    var idProp = relatedDto.GetType().GetProperty("Id");
                    if (idProp == null) continue;
                    var id = idProp.GetValue(relatedDto);
                    if (id == null) continue;
                    var alreadyExists = entityNav.Cast<object>().Any(e => e.GetType().GetProperty("Id")?.GetValue(e).Equals(id) == true);
                    if (alreadyExists) continue;
                    // Use ChangeTracker to get tracked entity if available
                    var tracked = _context.ChangeTracker.Entries()
                        .FirstOrDefault(e => e.Entity.GetType() == entityType && (int)e.Property("Id").CurrentValue == (int)id)?.Entity;
                    if (tracked == null)
                    {
                        var trackedFind = findMethod.Invoke(dbSet, new object[] { new object[] { id } });
                        if (trackedFind != null)
                            tracked = trackedFind;
                    }
                    if (tracked != null)
                        entityNav.Add(tracked);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<TDto> FindByFieldAsync(string fieldName, object value)
        {
            var entityType = typeof(T);
            var property = entityType.GetProperty(fieldName);
            if (property == null)
                throw new ArgumentException($"Property '{fieldName}' not found on type '{entityType.Name}'");

            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "x");
            var propertyAccess = System.Linq.Expressions.Expression.Property(parameter, property);
            var constant = System.Linq.Expressions.Expression.Constant(value);
            var equality = System.Linq.Expressions.Expression.Equal(propertyAccess, System.Linq.Expressions.Expression.Convert(constant, property.PropertyType));
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equality, parameter);

            var entity = await _context.Set<T>().FirstOrDefaultAsync(lambda);
            return _mapper.Map<TDto>(entity);
        }
    }
}
