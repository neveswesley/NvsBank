using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace NvsBank.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger logger)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string message = ex.Message;

            switch (ex)
            {
                case ApplicationException:
                    statusCode = (int)HttpStatusCode.Unauthorized; // 401
                    break;
                case ArgumentException:          // ex: validação de parâmetros
                case FormatException:
                    statusCode = (int)HttpStatusCode.BadRequest; // 400
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError; // 500
                    message = "Internal server error";
                    logger.LogError(ex, "Unhandled exception");
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = statusCode,
                error = message
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
