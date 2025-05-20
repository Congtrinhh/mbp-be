using Application.Dtos.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IContractService : IBaseService<Contract, ContractDto>
    {
        /// <summary>
        /// Validates if an event time has any conflicts with existing MC contracts
        /// Throws TimeConflictException or BufferTimeWarningException if validation fails
        /// </summary>
        /// <param name="eventStart">Event start time</param>
        /// <param name="eventEnd">Event end time</param>
        /// <param name="mcId">MC ID</param>
        /// <param name="isIgnoreBufferCheck">If true, skip buffer time validation</param>
        Task ValidateEventTimeAsync(DateTime eventStart, DateTime eventEnd, int mcId, bool? isIgnoreBufferCheck = null);
    }
}
