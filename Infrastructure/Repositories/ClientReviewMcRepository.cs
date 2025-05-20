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
    public class ClientReviewMcRepository : BaseRepository<ClientReviewMc, ClientReviewMcDto>, IClientReviewMcRepository
    {
        public ClientReviewMcRepository(IDbConnection dbConnection, IMapper mapper) : base(dbConnection, mapper)
        {
        }

        public override async Task<PagedResponse<ClientReviewMcDto>> GetPagedAsync(PagedRequest pagedRequest)
        {
            if (pagedRequest is ClientReviewMcPagedRequest clientReviewMCPagedRequest && clientReviewMCPagedRequest.IsUseProc == true)
            {
                var parameters = new DynamicParameters();
                parameters.Add("$McId", clientReviewMCPagedRequest.McId);
                parameters.Add("$IsGetContract", clientReviewMCPagedRequest.IsGetContract);
                parameters.Add("$IsGetMc", clientReviewMCPagedRequest.IsGetMc);
                parameters.Add("$IsGetClient", clientReviewMCPagedRequest.IsGetClient);

                parameters.Add("$OrderBy", clientReviewMCPagedRequest.Sort);
                parameters.Add("$Limit", clientReviewMCPagedRequest.PageSize);
                parameters.Add("$Offset", clientReviewMCPagedRequest.PageIndex * clientReviewMCPagedRequest.PageSize);

                var multi = await DbConnection.QueryMultipleAsync("proc_client_review_mc_get_paged", parameters, commandType: CommandType.StoredProcedure);

                List<ClientReviewMc> reviews = multi.Read<ClientReviewMc>().ToList();
                var reviewDtos = Mapper.Map<List<ClientReviewMcDto>>(reviews);
                var totalCount = multi.ReadSingle<int>();

                var contracts = multi.Read<Contract>().ToList();
                var mcs = multi.Read<User>().ToList();
                var clients = multi.Read<User>().ToList();

                // Bind the one-to-one entities to the corresponding review objects
                foreach (var review in reviewDtos)
                {
                    int reviewId = review.Id;

                    if (clientReviewMCPagedRequest.IsGetContract != null && clientReviewMCPagedRequest.IsGetContract == true)
                    {
                        review.Contract = contracts.FirstOrDefault(c => c.Id == review.ContractId);
                    }

                    if (clientReviewMCPagedRequest.IsGetMc != null && clientReviewMCPagedRequest.IsGetMc == true)
                    {
                        review.Mc = mcs.FirstOrDefault(m => m.Id == review.McId);
                    }

                    if (clientReviewMCPagedRequest.IsGetClient != null && clientReviewMCPagedRequest.IsGetClient == true)
                    {
                        review.Client = clients.FirstOrDefault(c => c.Id == review.ClientId);
                    }
                }

                return new PagedResponse<ClientReviewMcDto>()
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
