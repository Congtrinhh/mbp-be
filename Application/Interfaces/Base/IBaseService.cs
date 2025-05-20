using Application.Dtos.User;
using Domain.Entities;
using Domain.Entities.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    /// <summary>
    /// Base service interface with common CRUD operations and user context
    /// created by: roo 20/04/2025
    /// </summary>
    public interface IBaseService<T, TDto> where T : BaseEntity where TDto : BaseEntity
    {
        /// <summary>
        /// Get entity by ID
        /// </summary>
        Task<TDto> GetByIdAsync(int id);

        /// <summary>
        /// Get all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Add new entity
        /// </summary>
        Task<int> AddAsync(TDto entity);

        /// <summary>
        /// Update existing entity
        /// </summary>
        Task<int> UpdateAsync(TDto entity);

        /// <summary>
        /// Delete entity by ID
        /// </summary>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Get paged results
        /// </summary>
        Task<PagedResponse<TDto>> GetPagedAsync(PagedRequest pagingRequest);

        /// <summary>
        /// Find entity by field value
        /// </summary>
        Task<TDto> FindByFieldAsync(string fieldName, object value);
    }
}
