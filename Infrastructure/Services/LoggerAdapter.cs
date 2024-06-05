using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;
public class LoggerAdapter<T> : ILoggerAdapter<T>
{
    private readonly ILogger<T> _logger;

    public LoggerAdapter(ILogger<T> logger)
    {
        _logger = logger;
    }

    public string? GetCallerName(T source)
    {
        return source?.GetType().FullName;
    }

    public void LogTrace(EventId eventId, string message, params object[] args)
    {
        if (!_logger.IsEnabled(LogLevel.Trace))
            return;
        _logger.LogTrace(eventId, message, args);
    }

    public void LogDebug(EventId eventId, string message, params object[] args)
    {
        if (!_logger.IsEnabled(LogLevel.Debug))
            return;
        _logger.LogDebug(eventId, message, args);
    }

    public void LogInformation(EventId eventId, string message, params object[] args)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
            return;
        _logger.LogInformation(eventId, message, args);
    }

    public void LogWarning(EventId eventId, string message, params object[] args)
    {
        if (!_logger.IsEnabled(LogLevel.Warning))
            return;
        _logger.LogWarning(eventId, message, args);
    }

    public void LogError(EventId eventId, string message, params object[] args)
    {
        if (!_logger.IsEnabled(LogLevel.Error))
            return;
        _logger.LogError(eventId, message, args);
    }

    public void LogError(Exception exception, EventId eventId, string message, params object[] args)
    {
        if (!_logger.IsEnabled(LogLevel.Error))
            return;
        _logger.LogError(eventId, exception, message, args);
    }
    public void LogCritical(Exception exception, EventId eventId, string message, params object[] args)
    {
        if (!_logger.IsEnabled(LogLevel.Critical))
            return;
        _logger.LogCritical(eventId, exception, message, args);
    }
}
