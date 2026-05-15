using BusTicketBooking.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.ViewComponents;

public sealed class DashboardSummaryViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string role)
    {
        var metrics = role switch
        {
            "Agency" => new[]
            {
                new DashboardMetric { Label = "Scope", Value = "Agency" },
                new DashboardMetric { Label = "Manage", Value = "Offices" },
                new DashboardMetric { Label = "Review", Value = "Bookings" }
            },
            "Office" => new[]
            {
                new DashboardMetric { Label = "Scope", Value = "Office" },
                new DashboardMetric { Label = "Manage", Value = "Buses" },
                new DashboardMetric { Label = "Review", Value = "Payments" }
            },
            "Customer" => new[]
            {
                new DashboardMetric { Label = "Scope", Value = "Customer" },
                new DashboardMetric { Label = "Search", Value = "Trips" },
                new DashboardMetric { Label = "Book", Value = "Seats" }
            },
            _ => new[]
            {
                new DashboardMetric { Label = "Status", Value = "Guest" },
                new DashboardMetric { Label = "Access", Value = "Limited" },
                new DashboardMetric { Label = "Next", Value = "Login" }
            }
        };

        return View(metrics);
    }
}
