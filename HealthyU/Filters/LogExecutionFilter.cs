using HealthyU.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace HealthyU.WebApi.Filters;

public class LogExecutionFilter(ILogger<LogExecutionFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var method = context.ActionDescriptor.DisplayName;
        var hasAttribute = context.ActionDescriptor.EndpointMetadata.OfType<LogExecutionAttribute>().FirstOrDefault();

        if (hasAttribute != null)
        {
            logger.LogInformation($"[START] Executing: {method} {hasAttribute.Message}");

            var stopwatch = Stopwatch.StartNew();
            await next();
            stopwatch.Stop();
            
            logger.LogInformation($"[END] Finished: {method} in {stopwatch.ElapsedMilliseconds} ms");
        }
        else
        {
            await next();
        }
    }
}
