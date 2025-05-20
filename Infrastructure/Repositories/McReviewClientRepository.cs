using Application.Dtos.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System.Data;
using Dapper;
using Domain.Entities.Paging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class McReviewClientRepository : BaseRepository<McReviewClient, McReviewClientDto>, IMcReviewClientRepository
    {
        public McReviewClientRepository(IDbConnection dbConnection, IMapper mapper) : base(dbConnection, mapper)
        {
        }

        public override async Task<PagedResponse<McReviewClientDto>> GetPagedAsync(PagedRequest pagedRequest)
        {
            if (pagedRequest is McReviewClientPagedRequest mcReviewClientPagedRequest && mcReviewClientPagedRequest.IsUseProc == true)
            {
                var parameters = new DynamicParameters();
                parameters.Add("$ClientId", mcReviewClientPagedRequest.ClientId);
                parameters.Add("$IsGetContract", mcReviewClientPagedRequest.IsGetContract);
                parameters.Add("$IsGetMc", mcReviewClientPagedRequest.IsGetMc);
                parameters.Add("$IsGetClient", mcReviewClientPagedRequest.IsGetClient);

                parameters.Add("$OrderBy", mcReviewClientPagedRequest.Sort);
                parameters.Add("$Limit", mcReviewClientPagedRequest.PageSize);
                parameters.Add("$Offset", mcReviewClientPagedRequest.PageIndex * mcReviewClientPagedRequest.PageSize);

                var multi = await DbConnection.QueryMultipleAsync("proc_mc_review_client_get_paged", parameters, commandType: CommandType.StoredProcedure);

                List<McReviewClient> reviews = multi.Read<McReviewClient>().ToList();
                var reviewDtos = Mapper.Map<List<McReviewClientDto>>(reviews);
                var totalCount = multi.ReadSingle<int>();

                var contracts = multi.Read<Contract>().ToList();
                var mcs = multi.Read<User>().ToList();
                var clients = multi.Read<User>().ToList();

                // Bind the one-to-one entities to the corresponding review objects
                foreach (var review in reviewDtos)
                {
                    int reviewId = review.Id;

                    if (mcReviewClientPagedRequest.IsGetContract != null && mcReviewClientPagedRequest.IsGetContract == true)
                    {
                        review.Contract = contracts.FirstOrDefault(c => c.Id == review.ContractId);
                    }

                    if (mcReviewClientPagedRequest.IsGetMc != null && mcReviewClientPagedRequest.IsGetMc == true)
                    {
                        review.Mc = mcs.FirstOrDefault(m => m.Id == review.McId);
                    }

                    if (mcReviewClientPagedRequest.IsGetClient != null && mcReviewClientPagedRequest.IsGetClient == true)
                    {
                        review.Client = clients.FirstOrDefault(c => c.Id == review.ClientId);
                    }
                }

                return new PagedResponse<McReviewClientDto>()
                {
                    Items = reviewDtos,
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
    }
}
