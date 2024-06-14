using core.Exceptions.Base;

namespace api.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (HttpExceptionBase e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, HttpExceptionBase exception)
    {
        context.Response.StatusCode = exception switch
        {
            BadRequestHttpExceptionBase => StatusCodes.Status400BadRequest,
            NotFoundHttpExceptionBase => StatusCodes.Status404NotFound,
            ConflictHttpExceptionBase => StatusCodes.Status409Conflict,
            ForbiddenHttpExceptionBase => StatusCodes.Status403Forbidden,
            UnauthorizedHttpExceptionBase => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = new { exception.Message };

        await context.Response.WriteAsJsonAsync(response);
    }
}