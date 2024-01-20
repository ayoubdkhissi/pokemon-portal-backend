using Microsoft.Extensions.Logging;

namespace Application.Services.Interfaces;
public interface ILoggerAdapter<T>
{
    string? GetCallerName(T source);
    void LogTrace(EventId eventId, string message, params object[] args);
    void LogDebug(EventId eventId, string message, params object[] args);
    void LogInformation(EventId eventId, string message, params object[] args);
    void LogWarning(EventId eventId, string message, params object[] args);
    void LogError(EventId eventId, string message, params object[] args);
    void LogError(Exception exception, EventId eventId, string message, params object[] args);
    void LogCritical(Exception exception, EventId eventId, string message, params object[] args);
}
