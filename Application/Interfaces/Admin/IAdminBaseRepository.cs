using Domain.Entities;
using Domain.Entities.Paging;

namespace Application.Interfaces.Admin
{
    /// <summary>
    /// Base interface for admin repositories
    /// Created by: tqcong 28/04/2025
    /// </summary>
    public interface IAdminBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<PagedResponse<TEntity>> GetPagedAsync(BaseAdminPagedRequest request);
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<int> SaveChangesAsync();
    }
}