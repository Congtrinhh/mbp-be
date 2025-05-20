using Application.Dtos.IdVerification;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IIdInfoRepository : IBaseRepository<IdInfo, IdInfoDto>
    {
        Task<IdInfo> GetByUserIdAsync(int userId);
        Task<bool> AddOrUpdateAsync(int userId, IdInfoDto idInfo);
    }
}