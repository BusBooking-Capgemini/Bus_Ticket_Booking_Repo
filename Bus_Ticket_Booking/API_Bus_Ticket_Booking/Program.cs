using API_Bus_Ticket_Booking.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB Context
builder.Services.AddDbContext<BusTicketBookingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Repositories
builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();

// Services
builder.Services.AddScoped<IBusService, BusService>();
builder.Services.AddScoped<IDriverService, DriverService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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