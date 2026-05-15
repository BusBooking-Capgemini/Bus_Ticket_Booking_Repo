using BusTicketBooking.Mvc.Helpers;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class DashboardController : Controller
{
    public IActionResult Index()
    {
        var model = new DashboardViewModel
        {
            Role = HttpContext.Session.GetString(SessionKeys.Role) ?? "Guest",
            Email = HttpContext.Session.GetString(SessionKeys.Email),
            DisplayName = HttpContext.Session.GetString(SessionKeys.DisplayName),
            AgencyId = GetInt(SessionKeys.AgencyId),
            OfficeId = GetInt(SessionKeys.OfficeId),
            CustomerId = GetInt(SessionKeys.CustomerId)
        };
        model.Actions = BuildActions(model);

        return View(model);
    }

    private int? GetInt(string key)
    {
        return int.TryParse(HttpContext.Session.GetString(key), out var value) ? value : null;
    }

    private static IReadOnlyList<DashboardActionViewModel> BuildActions(DashboardViewModel model)
    {
        return model.Role switch
        {
            "Agency" => new[]
            {
                new DashboardActionViewModel { Title = "Agency Profile", Text = "View agency details and summary.", Controller = "Agencies", Action = "Index" },
                new DashboardActionViewModel { Title = "Offices", Text = "Manage agency offices.", Controller = "Offices", Action = "Index" },
                new DashboardActionViewModel { Title = "Fleet", Text = "Review buses and drivers.", Controller = "Buses", Action = "Index" },
                new DashboardActionViewModel { Title = "Trips", Text = "Create and manage trips.", Controller = "Trips", Action = "Index" },
                new DashboardActionViewModel { Title = "Payments", Text = "Open revenue summary.", Controller = "Payment", Action = "RevenueSummary", RouteValues = RouteValues(("agencyId", model.AgencyId)) }
            },
            "Office" => new[]
            {
                new DashboardActionViewModel { Title = "Office Summary", Text = "View your office details.", Controller = "Offices", Action = "Index" },
                new DashboardActionViewModel { Title = "Buses", Text = "Manage office buses.", Controller = "Buses", Action = "Index" },
                new DashboardActionViewModel { Title = "Drivers", Text = "Manage office drivers.", Controller = "Drivers", Action = "Index" },
                new DashboardActionViewModel { Title = "Trips", Text = "Create and review office trips.", Controller = "Trips", Action = "Index" },
                new DashboardActionViewModel { Title = "Bookings", Text = "Open booking dashboard.", Controller = "Booking", Action = "Dashboard", RouteValues = RouteValues(("officeId", model.OfficeId)) }
            },
            "Customer" => new[]
            {
                new DashboardActionViewModel { Title = "Search Trips", Text = "Find buses by city and date.", Controller = "Trips", Action = "Search" },
                new DashboardActionViewModel { Title = "Book Ticket", Text = "Create a booking from a trip.", Controller = "Booking", Action = "Create" },
                new DashboardActionViewModel { Title = "Payment", Text = "Pay for an existing booking.", Controller = "Payment", Action = "Create" },
                new DashboardActionViewModel { Title = "Profile", Text = "View your customer profile.", Controller = "Customers", Action = "Profile" }
            },
            _ => new[]
            {
                new DashboardActionViewModel { Title = "Search Trips", Text = "Browse available trips.", Controller = "Trips", Action = "Search" },
                new DashboardActionViewModel { Title = "Customer Login", Text = "Sign in to book tickets.", Controller = "Auth", Action = "Login", RouteValues = new Dictionary<string, string> { ["role"] = "Customer" },
                },
                new DashboardActionViewModel { Title = "Customer Register", Text = "Create a customer account.", Controller = "Auth", Action = "CustomerRegister" }
            }
        };
    }

    private static IDictionary<string, string>? RouteValues(params (string Key, int? Value)[] values)
    {
        var routeValues = values
            .Where(value => value.Value.HasValue)
            .ToDictionary(value => value.Key, value => value.Value!.Value.ToString());

        return routeValues.Count == 0 ? null : routeValues;
    }
}
