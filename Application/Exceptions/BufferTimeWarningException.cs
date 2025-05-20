using System;
using Application.Dtos.ExceptionHandling;

namespace Application.Exceptions
{
    /// <summary>
    /// Exception thrown when events are within buffer time of each other.
    /// created by: tqcong 11/05/2025
    /// </summary>
    public class BufferTimeWarningException : Exception
    {
        public TimeConflictEventInfo NearbyEvent { get; }

        public BufferTimeWarningException(string message, int contractId, string eventName,
            DateTime eventStart, DateTime eventEnd)
            : base(message)
        {
            NearbyEvent = new TimeConflictEventInfo
            {
                Type = "BUFFER",
                ContractId = contractId,
                EventName = eventName,
                EventStart = eventStart,
                EventEnd = eventEnd
            };
        }
    }
}