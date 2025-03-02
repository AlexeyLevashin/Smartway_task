using Smartway_task.Exceptions;

namespace Smartway_task.Middlewares;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await ExceptionHandling(context, e);
        }
    }

    private async Task ExceptionHandling(HttpContext context, Exception e)
    {
        context.Response.StatusCode = e switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsJsonAsync(new { error = e.Message });
    }
}