using API_Bus_Ticket_Booking.Middleware;

using API_Bus_Ticket_Booking.Repositories;
using API_Bus_Ticket_Booking.Repositories.Interfaces;

using API_Bus_Ticket_Booking.Services;
using API_Bus_Ticket_Booking.Services.Interfaces;

using API_Bus_Ticket_Booking.Data;

using Microsoft.EntityFrameworkCore;

using AutoMapper;

using DotNetEnv;

namespace API_Bus_Ticket_Booking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var server = Environment.GetEnvironmentVariable("DB_SERVER");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var trustedConnection = Environment.GetEnvironmentVariable("DB_TRUSTED_CONNECTION");
            var trustCertificate = Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERTIFICATE");

            var connectionString =
                $"Server={server};Database={database};Trusted_Connection={trustedConnection};TrustServerCertificate={trustCertificate};";

            builder.Services.AddDbContext<BusTicketBookingContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            

            // Repository Dependency Injection
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            // Service Dependency Injection
            builder.Services.AddScoped<IBookingService, BookingService>();

            builder.Services.AddScoped<IPaymentService, PaymentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Global Exception Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}