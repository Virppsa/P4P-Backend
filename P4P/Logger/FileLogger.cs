using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace P4P.Logger;

public class FileLogger : ILogger
{
    private readonly FileLoggerConfiguration _configuration;
    private readonly CancellationToken _cancellationToken;
    private readonly BlockingCollection<string> _logQueue = new(new ConcurrentQueue<string>());

    public FileLogger(FileLoggerConfiguration configuration)
    {
        _configuration = configuration;
        _cancellationToken = new CancellationToken();

        Task.Run(WriteLogToFile, _cancellationToken);
    }

    // 3. Generics (in delegates, events and methods)(at least two)
    public IDisposable BeginScope<TState>(TState state) => null!;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    // 3. Generics (in delegates, events and methods)(at least two)
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        string logMessage = $"[{DateTime.UtcNow}] [{logLevel}] " + formatter(state, exception);

        if (exception != null)
        {
            logMessage += " " + exception.Message + Environment.NewLine + exception.StackTrace;
        }

        logMessage += Environment.NewLine;

        _logQueue.Add(logMessage);
    }

    private void WriteLogToFile()
    {
        string filePath = _configuration.DirectoryPath + $"p4p_{DateTime.Now:yyyy-dd-M}.log";
        
        Directory.CreateDirectory(_configuration.DirectoryPath);

        if (!File.Exists(filePath))
        {
            using (File.Create(filePath))
            {
            }
        }

        while (!_cancellationToken.IsCancellationRequested)
        {
            var logMessage = _logQueue.Take();

            File.AppendAllText(filePath, logMessage);
        }
    }
}
