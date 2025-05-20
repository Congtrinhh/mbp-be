using Application.Dtos.IdVerification;
using Application.Interfaces;
using AutoMapper;
using Dapper;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Repositories
{
    public class IdInfoRepository : BaseRepository<IdInfo, IdInfoDto>, IIdInfoRepository
    {
        private readonly IMapper _mapper;
        private readonly IDbConnection _dbConnection;

        public IdInfoRepository(IDbConnection dbConnection, IMapper mapper)
            : base(dbConnection, mapper)
        {
            _mapper = mapper;
            _dbConnection = dbConnection;
        }

        public async Task<IdInfo> GetByUserIdAsync(int userId)
        {
            var sql = @"
                SELECT * FROM id_info 
                WHERE user_id = @UserId AND is_active = 1";

            return await _dbConnection.QuerySingleOrDefaultAsync<IdInfo>(sql, new { UserId = userId });
        }

        public async Task<bool> AddOrUpdateAsync(int userId, IdInfoDto idInfoDto)
        {
            if (idInfoDto == null) return false;

            var existingInfo = await GetByUserIdAsync(userId);

            idInfoDto.UserId = userId;
            if (existingInfo == null)
            {
                return await AddAsync(idInfoDto) != 0;
            }

            idInfoDto.Id = existingInfo.Id;
            return await UpdateAsync(idInfoDto) > 0;
        }
    }
}