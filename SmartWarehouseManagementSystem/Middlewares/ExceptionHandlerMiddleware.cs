using FluentValidation;
using SmartWarehouseManagementSystem.Wrappers;
using System.Net;

namespace SmartWarehouseManagementSystem.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new ErrorResponse(StatusCodes.Status400BadRequest, ex.Errors.Select(e => e.ErrorMessage).ToList()));
            }
            catch (BadHttpRequestException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new ErrorResponse(StatusCodes.Status400BadRequest, [ex.Message]));
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(new ErrorResponse(StatusCodes.Status401Unauthorized, [ex.Message]));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new ErrorResponse(StatusCodes.Status500InternalServerError, [ex.Message]));
            }
        }
    }
}
