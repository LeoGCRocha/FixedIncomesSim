using System.Diagnostics;

namespace FixedIncome.API.Middlewares;

public class EndpointTimeLogger(RequestDelegate next, ILogger<EndpointTimeLogger> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatchTimer = Stopwatch.StartNew();

        await next(context);
        
        stopwatchTimer.Stop();
        logger.LogInformation("Run request {ARG0} in {ARG1} ms.", context.GetEndpoint() ,stopwatchTimer.ElapsedMilliseconds);
    }
}