using Application.Dtos.IdVerification;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserIdVerificationService : IBaseService<UserIdVerification, UserIdVerificationDto>
    {
        Task<UserIdVerificationDto> GetVerificationStatusAsync(int userId);
        Task<UserIdVerificationDto> UploadFacePhotoAsync(int userId, FacePhotoUploadDto photo);
        Task<UserIdVerificationDto> UploadIdCardPhotoAsync(int userId, IdCardPhotoUploadDto photo);
        Task<IdInfoDto> GetIdInfoAsync(int userId);
        Task<bool> ConfirmIdInfoAsync(int userId, IdInfoDto info);
        Task<bool> ValidatePhotosMatchAsync(byte[] faceImageBytes, byte[] idFrontImageBytes);
    }
}