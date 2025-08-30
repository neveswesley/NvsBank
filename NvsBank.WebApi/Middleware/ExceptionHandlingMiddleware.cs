namespace NvsBank.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (FluentValidation.ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors.Select(e => new
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage,
                Severity = e.Severity.ToString()
            });

            var result = new
            {
                StatusCode = 400,
                Title = "Validation Failed",
                Errors = errors
            };

            await context.Response.WriteAsJsonAsync(result);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var result = new
            {
                StatusCode = 500,
                Title = "Internal Server Error",
                Detail = ex.Message
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}