using Application.Dtos.IdVerification;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserIdVerificationRepository : IBaseRepository<UserIdVerification, UserIdVerificationDto>
    {
        Task<UserIdVerification> GetByUserIdAsync(int userId);
        Task<bool> UpdateStatusAsync(int userId, byte status, DateTime? verifiedAt = null);
        Task<bool> UpdateCurrentStepAsync(int userId, byte step);
        Task<bool> UpdateImagesAsync(int userId, string imageType, string imageUrl);
    }
}