using Newtonsoft.Json;
using P4P.Exceptions;
using System.Text.Json;

namespace P4P.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerFactory _loggerFactory;

    // 4. Delegates usage;
    public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _loggerFactory = loggerFactory;
    }

    // 6. Exceptions and dealing with them in a meaningfull way (most of the exceptions are logged to a file or a server);
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var logger = _loggerFactory.CreateLogger("File");
        try
        {
            await _next(httpContext);
        }
        catch (HttpException e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = e.StatusCode;

            await httpContext.Response.WriteAsync
                ($"{JsonConvert.SerializeObject(new {Message = e.Message})}");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
