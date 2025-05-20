using Domain.Entities;
using Domain.Entities.Paging;

namespace Application.Interfaces.Admin
{
    /// <summary>
    /// Base interface for admin services
    /// Created by: tqcong 28/04/2025
    /// </summary>
    public interface IAdminBaseService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : class
    {
        Task<PagedResponse<TDto>> GetPagedAsync(BaseAdminPagedRequest request);
        Task<TDto> GetByIdAsync(int id);
        Task<TDto> AddAsync(TDto dto);
        Task<TDto> UpdateAsync(int id, TDto dto);
        Task DeleteAsync(int id);
    }
}