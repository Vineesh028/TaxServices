

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;


namespace CongestionTaxServices.TestException
{


    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Writes the exception response json
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unexpected error occurred");
            var problemDetails = CreateProblemDetails(httpContext, exception);
            var json = ToJson(problemDetails);

            const string contentType = "application/problem+json";
            httpContext.Response.ContentType = contentType;
            await httpContext.Response.WriteAsync(json, cancellationToken);

            return true;
        }
        /// <summary>
        /// Creates problem details from exception
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
        {

            var statusCode = context.Response.StatusCode;
            var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = reasonPhrase,
                Type = exception.GetType().Name,
                Detail = exception.Message

            };

            return problemDetails;
        }

        /// <summary>
        /// Serialize object to json string
        /// </summary>
        /// <param name="problemDetails"></param>
        /// <returns></returns>
        private string ToJson(in ProblemDetails problemDetails)
        {
        try
        {
            return JsonConvert.SerializeObject(problemDetails);
        }
        catch (Exception ex)
        {
            const string msg = "An exception has occurred while serializing error to JSON";
            _logger.LogError(ex, msg);
        }

        return string.Empty;
    }
    }

}