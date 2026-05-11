using API_Bus_Ticket_Booking.Middleware;

using API_Bus_Ticket_Booking.Repositories;
using API_Bus_Ticket_Booking.Repositories.Interfaces;

using API_Bus_Ticket_Booking.Services;
using API_Bus_Ticket_Booking.Services.Interfaces;

using API_Bus_Ticket_Booking.Data;

using Microsoft.EntityFrameworkCore;

using FluentValidation;

using FluentValidation.AspNetCore;

using DotNetEnv;

namespace API_Bus_Ticket_Booking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            // Connection String From .env
            var connectionString =
                Environment.GetEnvironmentVariable(
                    "DB_CONNECTION_STRING");

            // DbContext
            builder.Services.AddDbContext<BusTicketBookingContext>(
                options =>
                    options.UseSqlServer(connectionString));

            // Controllers
            builder.Services.AddControllers();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Repository Dependency Injection
            builder.Services.AddScoped<
                IBookingRepository,
                BookingRepository>();

            builder.Services.AddScoped<
                IPaymentRepository,
                PaymentRepository>();

            // Service Dependency Injection
            builder.Services.AddScoped<
                IBookingService,
                BookingService>();

            builder.Services.AddScoped<
                IPaymentService,
                PaymentService>();

            var app = builder.Build();

            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }

            // Global Exception Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}