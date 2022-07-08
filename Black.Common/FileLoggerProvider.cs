using Microsoft.Extensions.Logging;

namespace Black.Common;

public class FileLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string category)
    {
        return new FileLogger();
    }
    public void Dispose()
    {

    }
}
