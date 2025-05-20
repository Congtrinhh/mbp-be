using Application.Interfaces;
using Dapper;
using Domain.Entities;
using Domain.Entities.Paging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Extensions;
using System.Data.Common;
using Application.Dtos.User;
using AutoMapper;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User, UserDto>, IUserRepository
    {
        public UserRepository(IDbConnection dbConnection, IMapper mapper) : base(dbConnection, mapper)
        {
            UseSnakeCaseMapping<Media>();
            UseSnakeCaseMapping<HostingStyle>();
            UseSnakeCaseMapping<McType>();
        }

        public override async Task<PagedResponse<UserDto>> GetPagedAsync(PagedRequest pagedRequest)
        {
            if (pagedRequest is UserPagedRequest userPagedRequest && userPagedRequest.IsUseProc == true)
            {
                // Create an instance of DynamicParameters to store the procedure parameters
                var parameters = new DynamicParameters();

                // Add the parameters from the UserPagedRequest object to the DynamicParameters instance
                parameters.Add("$FullName", userPagedRequest.FullName);
                parameters.Add("$Email", userPagedRequest.Email);
                parameters.Add("$PhoneNumber", userPagedRequest.PhoneNumber);
                parameters.Add("$IsMc", userPagedRequest.IsMc);
                parameters.Add("$IsVerified", userPagedRequest.IsVerified);
                parameters.Add("$IsNewbie", userPagedRequest.IsNewbie);
                parameters.Add("$NickName", userPagedRequest.NickName);
                parameters.Add("$MinAge", userPagedRequest.MinAge);
                parameters.Add("$MaxAge", userPagedRequest.MaxAge);
                parameters.Add("$IsGetMedia", userPagedRequest.IsGetMedia);
                parameters.Add("$IsGetMcType", userPagedRequest.IsGetMcType);
                parameters.Add("$IsGetProvince", userPagedRequest.IsGetProvince);
                parameters.Add("$McTypeIds", userPagedRequest.McTypeIds);
                parameters.Add("$HostingStyleIds", userPagedRequest.HostingStyleIds);
                parameters.Add("$ProvinceIds", userPagedRequest.ProvinceIds);
                parameters.Add("$Genders", userPagedRequest.Genders);

                parameters.Add("$OrderBy", userPagedRequest.Sort);
                parameters.Add("$Limit", userPagedRequest.PageSize);
                parameters.Add("$Offset", userPagedRequest.PageIndex * userPagedRequest.PageSize);


                // Call the stored procedure using Dapper's QueryMultipleAsync method
                var multi = await DbConnection.QueryMultipleAsync("proc_user_get_paged", parameters, commandType: CommandType.StoredProcedure);

                // Retrieve the results from the multiple select statements in the stored procedure
                List<User> users = multi.Read<User>().ToList();

                var userDtos = Mapper.Map<List<UserDto>>(users);

                var totalCount = multi.ReadSingle<int>();
                var mcTypes = multi.Read<McType>().ToList();
                var provinces = multi.Read<Province>().ToList();

                // Bind the media items and mc types to the corresponding user objects

                foreach (var user in userDtos)
                {
                    int userId = user.Id;

                    if (userPagedRequest.IsGetMcType != null && userPagedRequest.IsGetMcType == true)
                    {
                        user.McTypes = mcTypes.Where(m => m.McId == userId).ToList();
                    }

                    if (userPagedRequest.IsGetProvince != null && userPagedRequest.IsGetProvince == true)
                    {
                        user.Provinces = provinces.Where(m => m.UserId == userId).ToList();
                    }
                } 

                // Create a new instance of PagedResult<User> and return it
                return new PagedResponse<UserDto>()
                {
                    Items = userDtos,
                    TotalCount = totalCount,
                    PageIndex = pagedRequest.PageIndex,
                    PageSize = pagedRequest.PageSize
                };
            }
            else
            {
                return await base.GetPagedAsync(pagedRequest);
            }
        }

        public override async Task<UserDto> GetByIdAsync(int id)
        {
            //call mysql procedure name proc_user_get_by_id with param is id, the method returns multiple results
            var parameters = new DynamicParameters();
            parameters.Add("$Id", id);
                
            var multi = await DbConnection.QueryMultipleAsync("proc_user_get_by_id", parameters, commandType: CommandType.StoredProcedure);

            //get the first result
            var user = multi.ReadFirstOrDefault<User>();
            var userDto = Mapper.Map<UserDto>(user);
            //get hosting styles

            var hostingStyles = multi.Read<HostingStyle>().ToList();
            var mcTypes = multi.Read<McType>().ToList();
            var provinces = multi.Read<Province>().ToList();

            //binding hosting styles to user
            userDto.HostingStyles = hostingStyles;
            userDto.McTypes = mcTypes;
            userDto.Provinces = provinces;

            return userDto;
        }
    }
}
