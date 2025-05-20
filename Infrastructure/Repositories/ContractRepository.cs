﻿using Application.Dtos.User;
using Application.Interfaces;
using AutoMapper;
using Dapper;
using Domain.Entities;
using Domain.Entities.Paging;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ContractRepository : BaseRepository<Contract, ContractDto>, IContractRepository
    {
        public ContractRepository(IDbConnection dbConnection, IMapper mapper) : base(dbConnection, mapper)
        {
            UseSnakeCaseMapping<User>();
        }

        public override async Task<PagedResponse<ContractDto>> GetPagedAsync(PagedRequest basePagedRequest)
        {
            if (basePagedRequest is ContractPagedRequest pagedRequest && pagedRequest.IsUseProc == true)
            {
                //prepare parameters to call the stored procedure
                var parameters = new DynamicParameters();

                parameters.Add("$ClientId", pagedRequest.ClientId);
                parameters.Add("$McId", pagedRequest.McId);
                parameters.Add("$KeyWord", pagedRequest.KeyWord);
                parameters.Add("$Status", pagedRequest.Status);

                parameters.Add("$OrderBy", pagedRequest.Sort);
                parameters.Add("$Limit", pagedRequest.PageSize);
                parameters.Add("$Offset", pagedRequest.PageIndex * pagedRequest.PageSize);

                var multi = await DbConnection.QueryMultipleAsync("proc_contract_get_paged", parameters, commandType: CommandType.StoredProcedure);

                // get contracts
                List<Contract> contracts = multi.Read<Contract>().ToList();

                var contractDtos = Mapper.Map<List<ContractDto>>(contracts);

                var totalCount = multi.ReadSingle<int>();

                var mcs = multi.Read<User>().ToList();
                var clients = multi.Read<User>().ToList();

                // map users to contracts based on user id
                foreach (var contract in contractDtos)
                {
                    int clientId = (int)contract.ClientId!;
                    int mcId = (int)contract.McId!;
                    contract.Client = clients.FirstOrDefault(m => m.Id == clientId);
                    contract.Mc = mcs.FirstOrDefault(m => m.Id == mcId);
                }

                return new PagedResponse<ContractDto>()
                {
                    Items = contractDtos,
                    TotalCount = totalCount,
                    PageIndex = basePagedRequest.PageIndex,
                    PageSize = basePagedRequest.PageSize
                };
            }
            else
            {
                return await base.GetPagedAsync(basePagedRequest);
            }
        }

        public override async Task<ContractDto> GetByIdAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("$Id", id);

            var multi = await DbConnection.QueryMultipleAsync("proc_contract_get_by_id", parameters, commandType: CommandType.StoredProcedure);

            var contract = multi.ReadFirstOrDefault<Contract>();
            if (contract == null) throw new Exception("Contract not found");

            var mc = multi.ReadFirstOrDefault<User>();
            var client = multi.ReadFirstOrDefault<User>();

            var contractDto = Mapper.Map<ContractDto>(contract);
            contractDto.Mc = mc;
            contractDto.Client = client;

            return contractDto;
        }

        public async Task<IEnumerable<Contract>> GetContractsForReviewAsync()
        {
            var sql = @"
            SELECT * FROM contract
            WHERE is_active= true 
                AND event_end <= @CurrentDate
                AND is_remind = 0
                AND status = @Status";

            var parameters = new
            {
                CurrentDate = DateTime.UtcNow,
                Status = (int)ContractStatus.InEffect
            };

            return await DbConnection.QueryAsync<Contract>(sql, parameters);
        }

        public async Task<Contract> FindOverlappingEvent(DateTime eventStart, DateTime eventEnd, int mcId)
        {
            var sql = @"
                SELECT *
                FROM contract
                WHERE mc_id = @McId
                AND status = @Status
                AND (
                    (event_start <= @EventEnd AND event_end >= @EventStart)
                    OR (event_start >= @EventStart AND event_start <= @EventEnd)
                    OR (event_end >= @EventStart AND event_end <= @EventEnd)
                )
                LIMIT 1";

            return await DbConnection.QueryFirstOrDefaultAsync<Contract>(sql, new {
                McId = mcId,
                EventStart = eventStart,
                EventEnd = eventEnd,
                Status = ContractStatus.InEffect
            });
        }

        public async Task<Contract> FindBufferConflict(DateTime eventStart, DateTime eventEnd, int mcId)
        {
            const int BUFFER_MINUTES = 60;

            var sql = @"
                SELECT *
                FROM contract
                WHERE mc_id = @McId
                AND status = @Status
                AND (
                    ABS(TIMESTAMPDIFF(MINUTE, event_end, @EventStart)) <= @BufferMinutes
                    OR ABS(TIMESTAMPDIFF(MINUTE, @EventEnd, event_start)) <= @BufferMinutes
                )
                LIMIT 1";

            return await DbConnection.QueryFirstOrDefaultAsync<Contract>(sql, new {
                McId = mcId,
                EventStart = eventStart,
                EventEnd = eventEnd,
                Status = ContractStatus.InEffect,
                BufferMinutes = BUFFER_MINUTES
            });
        }
    }
}
