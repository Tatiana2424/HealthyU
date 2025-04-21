using HealthuU.BLL.Services.Interfaces.Logging;
using Microsoft.Extensions.Logging;

namespace HealthuU.BLL.Services.Realizations.Logging
{
    public class LoggerService<T> : ILoggerService<T>
    {
        private ILogger<T> _logger;
        private readonly Guid _instanceId;

        public LoggerService(ILogger<T> logger)
        {
            _logger = logger;
            _instanceId = Guid.NewGuid();
        }

        public void LogInformation(string msg)
        {
            _logger.Log(LogLevel.Information, $"[{_instanceId}] {msg}");
        }

        public void LogWarning(string msg)
        {
            _logger.Log(LogLevel.Warning, $"{msg}");
        }

        public void LogTrace(string msg)
        {
            _logger.Log(LogLevel.Trace, $"{msg}");
        }

        public void LogDebug(string msg)
        {
            _logger.Log(LogLevel.Debug, $"{msg}");
        }

        public void LogError(string msg)
        {
            _logger.Log(LogLevel.Error, $"{msg}");
        }
    }
}
