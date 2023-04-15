using Castle.DynamicProxy;
using Microsoft.AspNetCore.Razor.TagHelpers;
using P4P.Exceptions;

namespace P4P.Middleware;

public class LogAttempts : IInterceptor
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly Dictionary<string, DateTime> _dictionary = new Dictionary<string, DateTime>();

    public LogAttempts(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public void Intercept(IInvocation invocation)
    {
        var logger = _loggerFactory.CreateLogger("File");
        try
        {
            invocation.Proceed();
        }
        catch (AuthenticationException ex)
        {
            if(_dictionary.TryGetValue(ex.email, out DateTime lastLogin))
            {
                if ((ex.time - lastLogin).Seconds < 30)
                    logger.LogError($"{ex.email} - multiple unsuccessful login attempts");

            }
            _dictionary[ex.email] = ex.time;
            throw ex;
        }
    }
}
