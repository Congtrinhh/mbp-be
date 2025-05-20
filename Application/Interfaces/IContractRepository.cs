using Application.Dtos.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IContractRepository : IBaseRepository<Contract, ContractDto>
    {
        /// <summary>
        /// Get contracts that need to be reviewed
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Contract>> GetContractsForReviewAsync();

        /// <summary>
        /// Find overlapping events for an MC
        /// </summary>
        Task<Contract> FindOverlappingEvent(DateTime eventStart, DateTime eventEnd, int mcId);
        
        /// <summary>
        /// Find events within buffer time for an MC
        /// </summary>
        Task<Contract> FindBufferConflict(DateTime eventStart, DateTime eventEnd, int mcId);
    }
}
