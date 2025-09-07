using Application.Dtos.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Paging;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EFCoreUserRepository : EFCoreBaseRepository<User, UserDto>, IUserRepository
    {
        public EFCoreUserRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override Task<int> AddAsync(UserDto dto)
        {
            throw new System.NotImplementedException();
        }

        public override Task<int> DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public override async Task<PagedResponse<UserDto>> GetPagedAsync(PagedRequest pagedRequest)
        {
            var request = (UserPagedRequest)pagedRequest;

            if (request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }

            var mcRatingQuery = _context.ClientReviewMcs.GroupBy(r => r.McId)
                                        .Select(g => new { UserId = g.Key, AvgRating = (decimal?)g.Average(r => r.OverallPoint) ?? 0, ReviewCount = g.Count() });
            var clientRatingQuery = _context.McReviewClients.GroupBy(r => r.ClientId)
                                        .Select(g => new { UserId = g.Key, AvgRating = (decimal?)g.Average(r => r.OverallPoint) ?? 0, ReviewCount = g.Count() });

            var query = from user in _context.Users
                        join mcRating in mcRatingQuery on user.Id equals mcRating.UserId into mcRatings
                        from mcRating in mcRatings.DefaultIfEmpty()
                        join clientRating in clientRatingQuery on user.Id equals clientRating.UserId into clientRatings
                        from clientRating in clientRatings.DefaultIfEmpty()
                        select new
                        {
                            User = user,
                            AvgRating = user.IsMc ? (mcRating.AvgRating) : (clientRating.AvgRating),
                            ReviewCount = (int?)(user.IsMc ? (mcRating.ReviewCount) : (clientRating.ReviewCount)) ?? 0
                        };

            if (!string.IsNullOrEmpty(request.FullName))
            {
                query = query.Where(u => u.User.FullName.Contains(request.FullName));
            }

            if (request.IsMc.HasValue)
            {
                query = query.Where(u => u.User.IsMc == request.IsMc.Value);
            }

            if (request.McTypeIds != null && request.McTypeIds.Any())
            {
                query = query.Where(u => u.User.McTypes.Any(mt => request.McTypeIds.Contains(mt.Id.ToString())));
            }

            if (request.ProvinceIds != null && request.ProvinceIds.Any())
            {
                query = query.Where(u => u.User.Provinces.Any(p => request.ProvinceIds.Contains(p.Id.ToString())));
            }

            var totalCount = await query.CountAsync();

            var pagedQuery = query.OrderBy(p => p.User.Id)
                                  .Skip((request.PageIndex - 1) * request.PageSize)
                                  .Take(request.PageSize);

            var pagedResult = await pagedQuery
                .Select(q => new UserDto
                {
                    Id = q.User.Id,
                    FullName = q.User.FullName,
                    Email = q.User.Email,
                    PhoneNumber = q.User.PhoneNumber,
                    IsMc = q.User.IsMc,
                    Age = q.User.Age,
                    NickName = q.User.NickName,
                    Gender = q.User.Gender,
                    IsNewbie = q.User.IsNewbie,
                    IsVerified = q.User.IsVerified,
                    Description = q.User.Description,
                    Education = q.User.Education,
                    Height = q.User.Height,
                    Weight = q.User.Weight,
                    AvatarUrl = q.User.AvatarUrl,
                    Facebook = q.User.Facebook,
                    Zalo = q.User.Zalo,
                    McTypes = q.User.McTypes.ToList(),
                    Provinces = q.User.Provinces.ToList(),
                    HostingStyles = q.User.HostingStyles.ToList(),
                    AvgRating = q.AvgRating,
                    ReviewCount = q.ReviewCount
                })
                .ToListAsync();

            return new PagedResponse<UserDto>(pagedResult, totalCount, request.PageIndex, request.PageSize);
        }

        public override async Task<UserDto> GetByIdAsync(int id)
        {
            var mcRatingQuery = _context.ClientReviewMcs.Where(r => r.McId == id)
                                        .GroupBy(r => r.McId)
                                        .Select(g => new { AvgRating = (decimal?)g.Average(r => r.OverallPoint) ?? 0, ReviewCount = g.Count() });

            var clientRatingQuery = _context.McReviewClients.Where(r => r.ClientId == id)
                                        .GroupBy(r => r.ClientId)
                                        .Select(g => new { AvgRating = (decimal?)g.Average(r => r.OverallPoint) ?? 0, ReviewCount = g.Count() });

            var user = await _context.Users
                .Include(u => u.McTypes)
                .Include(u => u.Provinces)
                .Include(u => u.HostingStyles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            var userDto = _mapper.Map<UserDto>(user);

            if (user.IsMc)
            {
                var mcRating = await mcRatingQuery.FirstOrDefaultAsync();
                if (mcRating != null)
                {
                    userDto.AvgRating = mcRating.AvgRating;
                    userDto.ReviewCount = mcRating.ReviewCount;
                }
            }
            else
            {
                var clientRating = await clientRatingQuery.FirstOrDefaultAsync();
                if (clientRating != null)
                {
                    userDto.AvgRating = clientRating.AvgRating;
                    userDto.ReviewCount = clientRating.ReviewCount;
                }
            }

            return userDto;
        }

        //public override async Task<int> UpdateAsync(UserDto userDto)
        //{
        //    var user = await _context.Users
        //        .Include(u => u.McTypes)
        //        .Include(u => u.Provinces)
        //        .Include(u => u.HostingStyles)
        //        .SingleAsync(u => u.Id == userDto.Id);

        //    _mapper.Map(userDto, user);

        //    // Reconcile McTypes
        //    var newMcTypeIds = userDto.McTypes.Select(t => t.Id).ToList();
        //    var mcTypesToRemove = user.McTypes.Where(t => !newMcTypeIds.Contains(t.Id)).ToList();
        //    foreach (var mcType in mcTypesToRemove)
        //    {
        //        user.McTypes.Remove(mcType);
        //    }
        //    var existingMcTypeIds = user.McTypes.Select(t => t.Id);
        //    var mcTypesToAdd = await _context.McTypes.Where(t => newMcTypeIds.Except(existingMcTypeIds).Contains(t.Id)).ToListAsync();
        //    mcTypesToAdd.ForEach(t => user.McTypes.Add(t));

        //    // Reconcile Provinces
        //    var newProvinceIds = userDto.Provinces.Select(p => p.Id).ToList();
        //    var provincesToRemove = user.Provinces.Where(p => !newProvinceIds.Contains(p.Id)).ToList();
        //    foreach (var province in provincesToRemove)
        //    {
        //        user.Provinces.Remove(province);
        //    }
        //    var existingProvinceIds = user.Provinces.Select(p => p.Id);
        //    var provincesToAdd = await _context.Provinces.Where(p => newProvinceIds.Except(existingProvinceIds).Contains(p.Id)).ToListAsync();
        //    provincesToAdd.ForEach(p => user.Provinces.Add(p));

        //    // Reconcile HostingStyles
        //    var newHostingStyleIds = userDto.HostingStyles.Select(h => h.Id).ToList();
        //    var hostingStylesToRemove = user.HostingStyles.Where(h => !newHostingStyleIds.Contains(h.Id)).ToList();
        //    foreach (var hostingStyle in hostingStylesToRemove)
        //    {
        //        user.HostingStyles.Remove(hostingStyle);
        //    }
        //    var existingHostingStyleIds = user.HostingStyles.Select(h => h.Id);
        //    var hostingStylesToAdd = await _context.HostingStyles.Where(h => newHostingStyleIds.Except(existingHostingStyleIds).Contains(h.Id)).ToListAsync();
        //    hostingStylesToAdd.ForEach(h => user.HostingStyles.Add(h));

        //    return await _context.SaveChangesAsync();
        //}
    }
}
