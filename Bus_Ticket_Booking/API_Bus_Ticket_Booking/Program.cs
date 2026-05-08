using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Repositories;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Api_Bus_Ticket_Booking.Services;
using API_Bus_Ticket_Booking.Services;
using API_Bus_Ticket_Booking.Services.Interfaces;
using DotNetEnv;
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
        );

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(Program));

        // Add services to the container.
        builder.Services.AddScoped<IBusRepository, BusRepository>();
        builder.Services.AddScoped<IDriverRepository, DriverRepository>();

        // Services
        builder.Services.AddScoped<IBusService, BusService>();
        builder.Services.AddScoped<IDriverService, DriverService>();

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

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
