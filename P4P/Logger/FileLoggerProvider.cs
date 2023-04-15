using Microsoft.Extensions.Options;

namespace P4P.Logger;

[ProviderAlias("File")]
public class FileLoggerProvider : ILoggerProvider
{
    private FileLoggerConfiguration _configuration;
    private readonly IDisposable _onChangeToken;

    public FileLoggerProvider(IOptionsMonitor<FileLoggerConfiguration> configurationMonitor)
    {
        _configuration = configurationMonitor.CurrentValue;
        _onChangeToken = configurationMonitor.OnChange(updatedConfiguration => _configuration = updatedConfiguration);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(_configuration);
    }

    public void Dispose()
    {
        _onChangeToken.Dispose();
        GC.SuppressFinalize(this);
    }
}
