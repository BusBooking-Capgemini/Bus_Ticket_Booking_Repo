using API_Bus_Ticket_Booking.Exceptions;

using FluentValidation;

using System.Net;
using System.Text.Json;

namespace API_Bus_Ticket_Booking.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware>
            _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;

            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    ex.Message);

                await HandleExceptionAsync(
                    context,
                    ex);
            }
        }

        private static async Task
            HandleExceptionAsync(
                HttpContext context,
                Exception exception)
        {
            context.Response.ContentType =
                "application/json";

            object response;

            switch (exception)
            {
                case FluentValidation.ValidationException validationException:

                    context.Response.StatusCode =
                        (int)HttpStatusCode
                            .BadRequest;

                    response = new
                    {
                        success = false,

                        statusCode =
                            context.Response.StatusCode,

                        errors =
                            validationException.Errors
                                .Select(x => new
                                {
                                    field =
                                        x.PropertyName,

                                    message =
                                        x.ErrorMessage
                                })
                    };

                    break;

                case NotFoundException:

                    context.Response.StatusCode =
                        (int)HttpStatusCode
                            .NotFound;

                    response = new
                    {
                        success = false,

                        statusCode =
                            context.Response.StatusCode,

                        message =
                            exception.Message
                    };

                    break;

                case BadRequestException:

                    context.Response.StatusCode =
                        (int)HttpStatusCode
                            .BadRequest;

                    response = new
                    {
                        success = false,

                        statusCode =
                            context.Response.StatusCode,

                        message =
                            exception.Message
                    };

                    break;

                case ConflictException:

                    context.Response.StatusCode =
                        (int)HttpStatusCode
                            .Conflict;

                    response = new
                    {
                        success = false,

                        statusCode =
                            context.Response.StatusCode,

                        message =
                            exception.Message
                    };

                    break;

                case UnauthorizedException:

                    context.Response.StatusCode =
                        (int)HttpStatusCode
                            .Unauthorized;

                    response = new
                    {
                        success = false,

                        statusCode =
                            context.Response.StatusCode,

                        message =
                            exception.Message
                    };

                    break;

                case ForbiddenException:

                    context.Response.StatusCode =
                        (int)HttpStatusCode
                            .Forbidden;

                    response = new
                    {
                        success = false,

                        statusCode =
                            context.Response.StatusCode,

                        message =
                            exception.Message
                    };

                    break;

                case BusinessException:

                    context.Response.StatusCode =
                        422;

                    response = new
                    {
                        success = false,

                        statusCode =
                            context.Response.StatusCode,

                        message =
                            exception.Message
                    };

                    break;

                default:

                    context.Response.StatusCode =
                        (int)HttpStatusCode
                            .InternalServerError;

                    response = new
                    {
                        success = false,

                        statusCode =
                            context.Response.StatusCode,

                        message =
                            "Internal Server Error"
                    };

                    break;
            }

            var jsonResponse =
                JsonSerializer.Serialize(
                    response);

            await context.Response
                .WriteAsync(jsonResponse);
        }
    }
}