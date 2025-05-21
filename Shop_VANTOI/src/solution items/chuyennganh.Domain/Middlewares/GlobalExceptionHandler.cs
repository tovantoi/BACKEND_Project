using chuyennganh.Domain.DependencyInjection.Extensions;
using chuyennganh.Domain.ExceptionEx;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Domain.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;
        private readonly IWebHostEnvironment env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error Message: {exceptionMessage}, Time of occurrence {time}", exception.Message, DateTime.UtcNow);

            // Check current environment is production or not (can be development, QA, Test)
            var isProduction = env.IsProduction();
            var result = exception is ShopException customException ? customException.ConvertToResult() : exception.ConvertToResult();
            result.Error = isProduction ? null : result.Error;

            // Set response status code synchronous with result status code
            context.Response.StatusCode = result.StatusCode;

            // Write result to response as application/json format
            await context.Response.WriteAsJsonAsync(result, cancellationToken);

            // Return true mark that exception has been handled and not go to other routes
            return true;
        }
    }
}