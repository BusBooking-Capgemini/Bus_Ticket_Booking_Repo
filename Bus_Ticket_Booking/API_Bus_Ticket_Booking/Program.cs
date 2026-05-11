using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Helpers.JWT;
using API_Bus_Ticket_Booking.Middleware;

using API_Bus_Ticket_Booking.Repositories;
using API_Bus_Ticket_Booking.Repositories.Interfaces;

using API_Bus_Ticket_Booking.Services;
using API_Bus_Ticket_Booking.Services.Interfaces;

using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;

using Microsoft.OpenApi.Models;

using System.Text;

namespace API_Bus_Ticket_Booking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            // CONNECTION STRING
            var connectionString =
                Environment.GetEnvironmentVariable(
                    "DB_CONNECTION_STRING");

            // JWT SETTINGS
            var jwtKey =
                Environment.GetEnvironmentVariable(
                    "JWT_KEY");

            var jwtIssuer =
                Environment.GetEnvironmentVariable(
                    "JWT_ISSUER");

            var jwtAudience =
                Environment.GetEnvironmentVariable(
                    "JWT_AUDIENCE");

            // DB CONTEXT
            builder.Services.AddDbContext<
                BusTicketBookingContext>(
                options =>
                    options.UseSqlServer(
                        connectionString));

            // CONTROLLERS
            builder.Services.AddControllers();

            // FLUENT VALIDATION
            builder.Services
                .AddFluentValidationAutoValidation();

            builder.Services
                .AddValidatorsFromAssemblyContaining<
                    Program>();

            // AUTOMAPPER
            builder.Services.AddAutoMapper(
                typeof(Program));

            // JWT AUTHENTICATION
            builder.Services
                .AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,

                            ValidateAudience = true,

                            ValidateLifetime = true,

                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtIssuer,

                            ValidAudience = jwtAudience,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        jwtKey!))
                        };
                });

            // AUTHORIZATION
            builder.Services.AddAuthorization();

            // SWAGGER
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title =
                            "Bus Ticket Booking API",

                        Version = "v1"
                    });

                // JWT SWAGGER SUPPORT
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",

                        Type = SecuritySchemeType.Http,

                        Scheme = "bearer",

                        BearerFormat = "JWT",

                        In = ParameterLocation.Header,

                        Description =
                            "Enter Token Like: Bearer {token}"
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference =
                                    new OpenApiReference
                                    {
                                        Type =
                                            ReferenceType
                                                .SecurityScheme,

                                        Id = "Bearer"
                                    }
                            },

                            Array.Empty<string>()
                        }
                    });
            });

            // REPOSITORY DEPENDENCY INJECTION

            builder.Services.AddScoped<
                IBookingRepository,
                BookingRepository>();

            builder.Services.AddScoped<
                IPaymentRepository,
                PaymentRepository>();

            builder.Services.AddScoped<
                IAuthRepository,
                AuthRepository>();

            // SERVICE DEPENDENCY INJECTION

            builder.Services.AddScoped<
                IBookingService,
                BookingService>();

            builder.Services.AddScoped<
                IPaymentService,
                PaymentService>();

            builder.Services.AddScoped<
                IAuthService,
                AuthService>();

            // JWT HELPER
            builder.Services.AddScoped<
                JwtHelper>();

            var app = builder.Build();

            // SWAGGER
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }

            // GLOBAL EXCEPTION MIDDLEWARE
            app.UseMiddleware<
                ExceptionMiddleware>();

            app.UseHttpsRedirection();

            // AUTHENTICATION
            app.UseAuthentication();

            // AUTHORIZATION
            app.UseAuthorization();

            // MAP CONTROLLERS
            app.MapControllers();

            app.Run();
        }
    }
}