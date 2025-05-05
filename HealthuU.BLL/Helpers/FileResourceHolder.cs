using HealthuU.BLL.Services.Interfaces.Logging;

namespace HealthuU.BLL.Helpers;

public class FileResourceHolder : IDisposable
{
    private StreamReader? _reader;
    private bool _disposed = false;
    private readonly ILoggerService<FileResourceHolder> _logger;

    public FileResourceHolder(string filePath, ILoggerService<FileResourceHolder> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        try
        {
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            _reader = new StreamReader(stream);

            logger.LogInformation($"File opened: {filePath}");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error opening file: {ex.Message}");
            throw;
        }
    }

    public async Task<string> ReadAllTextAsync()
    {
        return _disposed || _reader is null 
            ? throw new ObjectDisposedException(nameof(FileResourceHolder)) 
            : await _reader.ReadToEndAsync();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _reader?.Dispose();
        _reader = null;
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    ~FileResourceHolder()
    {
        Dispose();
    }
}

