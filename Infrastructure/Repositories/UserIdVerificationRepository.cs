using Application.Dtos.IdVerification;
using Application.Interfaces;
using AutoMapper;
using Dapper;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Repositories
{
    public class UserIdVerificationRepository : BaseRepository<UserIdVerification, UserIdVerificationDto>, IUserIdVerificationRepository
    {
        private readonly IIdInfoRepository _idInfoRepository;

        public UserIdVerificationRepository(
            IDbConnection dbConnection, 
            IMapper mapper,
            IIdInfoRepository idInfoRepository) 
            : base(dbConnection, mapper)
        {
            _idInfoRepository = idInfoRepository;
        }

        public async Task<UserIdVerification> GetByUserIdAsync(int userId)
        {
            var sql = @"
                SELECT * FROM user_id_verification 
                WHERE user_id = @UserId AND is_active = 1";

            var verification = await DbConnection.QuerySingleOrDefaultAsync<UserIdVerification>(sql, new { UserId = userId });
            
            if (verification != null)
            {
                verification.IdInfo = await _idInfoRepository.GetByUserIdAsync(userId);
            }

            return verification;
        }

        public async Task<bool> UpdateStatusAsync(int userId, byte status, DateTime? verifiedAt)
        {
            var sql = @"
                UPDATE user_id_verification 
                SET status = @Status,
                    verified_at = @VerifiedAt,
                    modified_at = NOW()
                WHERE user_id = @UserId AND is_active = 1";

            var result = await DbConnection.ExecuteAsync(sql, new { 
                UserId = userId, 
                Status = status, 
                VerifiedAt = verifiedAt 
            });

            return result > 0;
        }

        public async Task<bool> UpdateCurrentStepAsync(int userId, byte step)
        {
            var verification = await GetByUserIdAsync(userId);
            if (verification == null)
            {
                var sql = @"
                    INSERT INTO user_id_verification (
                        user_id, current_step, status, created_at, modified_at, is_active
                    ) VALUES (
                        @UserId, @Step, 0, NOW(), NOW(), 1
                    )";

                var result = await DbConnection.ExecuteAsync(sql, new { UserId = userId, Step = step });
                return result > 0;
            }

            var updateSql = @"
                UPDATE user_id_verification 
                SET current_step = @Step,
                    modified_at = NOW()
                WHERE user_id = @UserId AND is_active = 1";

            var updateResult = await DbConnection.ExecuteAsync(updateSql, new { UserId = userId, Step = step });
            return updateResult > 0;
        }

        public async Task<bool> UpdateImagesAsync(int userId, string imageType, string imageUrl)
        {
            var columnName = imageType switch
            {
                "face" => "face_image_url",
                "front" => "id_front_image_url",
                "back" => "id_back_image_url",
                _ => throw new ArgumentException("Invalid image type")
            };

            var verification = await GetByUserIdAsync(userId);
            if (verification == null)
            {
                var sql = $@"
                    INSERT INTO user_id_verification (
                        user_id, {columnName}, current_step, status, created_at, modified_at, is_active
                    ) VALUES (
                        @UserId, @ImageUrl, 0, 0, NOW(), NOW(), 1
                    )";

                var result = await DbConnection.ExecuteAsync(sql, new { UserId = userId, ImageUrl = imageUrl });
                return result > 0;
            }

            var updateSql = $@"
                UPDATE user_id_verification 
                SET {columnName} = @ImageUrl,
                    modified_at = NOW()
                WHERE user_id = @UserId AND is_active = 1";

            var updateResult = await DbConnection.ExecuteAsync(updateSql, new { UserId = userId, ImageUrl = imageUrl });
            return updateResult > 0;
        }
    }
}