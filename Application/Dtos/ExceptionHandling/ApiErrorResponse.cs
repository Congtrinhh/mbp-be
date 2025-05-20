using System;
using System.Collections.Generic;

namespace Application.Dtos.ExceptionHandling
{
    /// <summary>
    /// Generic error response format for API exceptions.
    /// created by: tqcong 11/05/2025
    /// </summary>
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<object> AdditionalInfo { get; set; }
        public string TraceId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}