using System;
using Application.Dtos.ExceptionHandling;

namespace Application.Exceptions
{
    /// <summary>
    /// Exception thrown when an MC has overlapping events.
    /// created by: tqcong 11/05/2025
    /// </summary>
    public class TimeConflictException : Exception
    {
        public TimeConflictEventInfo ConflictingEvent { get; }

        public TimeConflictException(string message, int contractId, string eventName,
            DateTime eventStart, DateTime eventEnd)
            : base(message)
        {
            ConflictingEvent = new TimeConflictEventInfo
            {
                Type = "CONFLICT",
                ContractId = contractId,
                EventName = eventName,
                EventStart = eventStart,
                EventEnd = eventEnd
            };
        }
    }
}