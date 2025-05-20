﻿using Application.Dtos.ExceptionHandling;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            NotSameUserException conflictEx => new ApiErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                Message = conflictEx.Message,
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            },
            TimeConflictException conflictEx => new ApiErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Conflict, // 409
                Message = conflictEx.Message,
                AdditionalInfo = new List<object> { conflictEx.ConflictingEvent },
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            },
            BufferTimeWarningException warningEx => new ApiErrorResponse
            {
                StatusCode = 430, // Custom status code for buffer warning
                Message = warningEx.Message,
                AdditionalInfo = new List<object> { warningEx.NearbyEvent },
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            },
            _ => new ApiErrorResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = exception.Message,
                AdditionalInfo = new List<object>(),
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            }
        };

        context.Response.StatusCode = response.StatusCode;

        var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}