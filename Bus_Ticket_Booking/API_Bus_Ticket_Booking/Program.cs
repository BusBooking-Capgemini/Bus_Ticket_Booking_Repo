using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Repositories;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Implementations;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ── DbContext ─────────────────────────────────────────────────────
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ── Repositories ──────────────────────────────────────────────────
            builder.Services.AddScoped<IRouteRepository, RouteRepository>();
            builder.Services.AddScoped<ITripRepository, TripRepository>();

            // ── Services ──────────────────────────────────────────────────────
            builder.Services.AddScoped<IRouteService, RouteService>();
            builder.Services.AddScoped<ITripService, TripService>();

            var app = builder.Build();

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
}
