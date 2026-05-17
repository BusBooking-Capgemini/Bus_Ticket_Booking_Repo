using Bus_Ticket_Booking.Mvc.Services;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// SESSION
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;
});

// HTTP CONTEXT
builder.Services.AddHttpContextAccessor();

// HTTP CLIENT
builder.Services.AddHttpClient();

// SERVICES
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ITripService, TripService>();

builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddHttpClient<ICustomerService, CustomerService>();

builder.Services.AddScoped<IBusService, BusService>();

builder.Services.AddScoped<IDriverService, DriverService>();

builder.Services.AddScoped<IOfficeBookingService, OfficeBookingService>();

builder.Services.AddScoped<IOfficePaymentService, OfficePaymentService>();

builder.Services.AddScoped<IOfficeDashboardService, OfficeDashboardService>();

builder.Services.AddScoped<IAgencyDashboardService, AgencyDashboardService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

var app = builder.Build();

// ERROR HANDLING
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// DEFAULT ROUTE
app.MapControllerRoute(name: "default", pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
