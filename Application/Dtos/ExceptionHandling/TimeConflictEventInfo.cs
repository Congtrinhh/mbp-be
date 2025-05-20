using System;

namespace Application.Dtos.ExceptionHandling
{
    /// <summary>
    /// Information about conflicting events for time validation.
    /// Type values:
    /// - CONFLICT: Direct time overlap with another event
    /// - BUFFER: Event is within 1-hour buffer time of another event
    /// created by: tqcong 11/05/2025
    /// </summary>
    public class TimeConflictEventInfo
    {
        public string Type { get; set; }
        public int ContractId { get; set; }
        public string EventName { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
    }
}