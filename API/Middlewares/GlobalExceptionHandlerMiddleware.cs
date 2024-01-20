using API.Common;
using Application.Services.Interfaces;
using Domain.Enums;
using Shared.Constants;
using System.Net;
using System.Text.Json;

namespace API.Middlewares;
public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILoggerAdapter<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(ILoggerAdapter<GlobalExceptionHandlerMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);
        try
        {
            await next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex).ConfigureAwait(false);
        }
    }

    private async Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, new EventId((int)LogEvents.GlobalException), "Exception Handeled by Global ExceptionHandler");
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var apiResponse = new ApiResponse<string>
        {
            Success = false,
            ErrorCode = ErrorCodes.InternalServerError
        };
        var json = JsonSerializer.Serialize(apiResponse);
        await context.Response.WriteAsync(json).ConfigureAwait(false);
        context.Response.ContentType = Globals.jsonContentType;
    }
}