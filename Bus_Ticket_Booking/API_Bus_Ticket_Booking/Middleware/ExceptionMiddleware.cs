using API_Bus_Ticket_Booking.Exceptions;
using System.Net;
using System.Text.Json;

namespace API_Bus_Ticket_Booking.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = exception.Message
            };

            switch (exception)
            {
                case NotFoundException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.NotFound;
                    break;

                case BadRequestException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.BadRequest;
                    break;

                case ValidationException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.BadRequest;
                    break;

                case ConflictException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.Conflict;
                    break;

                case UnauthorizedException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.Unauthorized;
                    break;

                case ForbiddenException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.Forbidden;
                    break;

                case BusinessException:
                    context.Response.StatusCode = 422;
                    break;

                default:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
