using System.Net;
using Microsoft.EntityFrameworkCore;
using TransactionWorkflowEngine.Dtos;

namespace TransactionWorkflowEngine.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            } catch (Exception ex)
            {
                var (statusCode, title) = MapException(ex);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var errorResponse = new ErrorResponseDto
                {
                    StatusCode = statusCode,
                    Title = title,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

        private static (int StatusCode, string Title) MapException(Exception ex)
        {
            return ex switch
            {
                DbUpdateConcurrencyException => ((int)HttpStatusCode.Conflict, "Conflict"),
                InvalidOperationException => ((int)HttpStatusCode.BadRequest, "Bad Request"),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "Not Found"),
                _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error")
            };
        }
    }
}
