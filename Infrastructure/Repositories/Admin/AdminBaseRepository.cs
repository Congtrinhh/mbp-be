using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Entities.Paging;
using Domain.Extensions;
using Application.Interfaces.Admin;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Admin
{
    /// <summary>
    /// Base repository implementation for admin operations using EF Core
    /// Created by: tqcong 28/04/2025
    /// </summary>
    public class AdminBaseRepository<TEntity> : IAdminBaseRepository<TEntity> 
        where TEntity : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public AdminBaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            // Use FirstOrDefaultAsync with explicit loading to handle null values better
            return await _dbSet
                .AsNoTracking()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<PagedResponse<TEntity>> GetPagedAsync(BaseAdminPagedRequest request)
        {
            var query = _dbSet.AsQueryable();

            // Handle admin-specific search if request is BaseAdminPagedRequest
            if (request is BaseAdminPagedRequest adminRequest)
            {
                if (!string.IsNullOrEmpty(adminRequest.Search) && adminRequest.SearchFields?.Any() == true)
                {
                    var searchValue = adminRequest.Search.ToLower();
                    Expression<Func<TEntity, bool>> searchPredicate = null;

                    foreach (var requestedField in adminRequest.SearchFields)
                    {
                        // Find the actual property name using case-insensitive comparison
                        var propertyInfo = typeof(TEntity).GetProperties()
                            .FirstOrDefault(p => p.Name.Equals(requestedField, StringComparison.OrdinalIgnoreCase));
                        if (propertyInfo == null) continue;

                        var parameter = Expression.Parameter(typeof(TEntity), "x");
                        var property = Expression.Property(parameter, propertyInfo);

                        // MySQL friendly case-insensitive search
                        var toLower = Expression.Call(property, "ToLower", null);
                        var contains = Expression.Call(toLower, "Contains", null, Expression.Constant(searchValue));
                        var lambda = Expression.Lambda<Func<TEntity, bool>>(contains, parameter);

                        searchPredicate = searchPredicate == null
                            ? lambda
                            : Expression.Lambda<Func<TEntity, bool>>(
                                Expression.OrElse(
                                    searchPredicate.Body,
                                    Expression.Invoke(lambda, searchPredicate.Parameters[0])
                                ),
                                searchPredicate.Parameters);
                    }

                    if (searchPredicate != null)
                    {
                        query = query.Where(searchPredicate);
                    }
                }
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortField))
            {
                var sortParts = request.SortField.Split(' ');
                var requestedField = sortParts[0];
                var isAscending = sortParts.Length == 1 || sortParts[1].ToLower() != "desc";

                // Find the actual property name using case-insensitive comparison
                var propertyInfo = typeof(TEntity).GetProperties()
                    .FirstOrDefault(p => p.Name.Equals(requestedField, StringComparison.OrdinalIgnoreCase));
                
                if (propertyInfo != null)
                {
                    var field = propertyInfo.Name; // Use the actual property name with correct casing

                    if (isAscending)
                    {
                        query = query.OrderBy(e => EF.Property<object>(e, field));
                    }
                    else
                    {
                        query = query.OrderByDescending(e => EF.Property<object>(e, field));
                    }
                }
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            if (request.PageSize != -1) // -1 means return all records
            {
                query = query
                    .Skip(request.PageIndex * request.PageSize)
                    .Take(request.PageSize);
            }

            var items = await query.ToListAsync();

            return new PagedResponse<TEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}