
using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Middleware;
using API_Bus_Ticket_Booking.Repositories;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services;
using API_Bus_Ticket_Booking.Services.Interfaces;
using API_Bus_Ticket_Booking.Validators;
using AutoMapper;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<BusTicketBookingContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            .UseLazyLoadingProxies()
        );

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(Program));

        // Repositories
        builder.Services.AddScoped<IAgencyRepository, AgencyRepository>();
        builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
        builder.Services.AddScoped<IBusRepository, BusRepository>();
        builder.Services.AddScoped<IDriverRepository, DriverRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IRouteRepository, RouteRepository>();
        builder.Services.AddScoped<ITripRepository, TripRepository>();

        // Services
        builder.Services.AddScoped<IAgencyService, AgencyService>();
        builder.Services.AddScoped<IOfficeService, OfficeService>();
        builder.Services.AddScoped<IBusService, BusService>();
        builder.Services.AddScoped<IDriverService, DriverService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<IReviewService, ReviewService>();
        builder.Services.AddScoped<IRouteService, RouteService>();
        builder.Services.AddScoped<ITripService, TripService>();

        // Validators
        builder.Services.AddFluentValidationAutoValidation();
        // Only need to register once, it will automatically regsiter all other validators.
        builder.Services.AddValidatorsFromAssemblyContaining<CustomerCreateDtoValidator>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}