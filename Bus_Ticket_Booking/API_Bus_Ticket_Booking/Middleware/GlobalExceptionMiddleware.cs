using API_Bus_Ticket_Booking.Exceptions;
using System.Net;
using System.Text.Json;

namespace API_Bus_Ticket_Booking.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                await WriteResponse(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex.Message);
                await WriteResponse(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (ConflictException ex)
            {
                _logger.LogWarning(ex.Message);
                await WriteResponse(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteResponse(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static async Task WriteResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                statusCode = (int)statusCode,
                message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}