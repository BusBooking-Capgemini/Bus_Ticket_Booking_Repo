using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class BookingController(IBookingService service) : Controller
{
    [HttpGet] public IActionResult Create() => View(new CreateBookingViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await service.CreateAsync(model);
        if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Message ?? "Booking failed."); return View(model); }
        return View("ApiDetails", new ApiJsonViewModel { Title = "Booking Created", ControllerName = "Booking", Result = result });
    }

    public async Task<IActionResult> Details(int id) => View("ApiDetails", new ApiJsonViewModel { Title = "Booking Details", ControllerName = "Booking", Result = await service.GetAsync(id) });
    public async Task<IActionResult> Dashboard(int? agencyId, int? officeId) => View("ApiDetails", new ApiJsonViewModel { Title = "Booking Dashboard", ControllerName = "Booking", Result = await service.DashboardAsync(agencyId, officeId) });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await service.CancelAsync(id);
        return View("ApiDetails", new ApiJsonViewModel { Title = "Booking Cancelled", ControllerName = "Booking", Result = result });
    }
}
