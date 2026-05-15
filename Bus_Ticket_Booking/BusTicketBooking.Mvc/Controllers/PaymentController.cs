using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class PaymentController(IPaymentService service) : Controller
{
    [HttpGet] public IActionResult Create() => View(new CreatePaymentViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePaymentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await service.CreateAsync(model);
        if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Message ?? "Payment failed."); return View(model); }
        return View("ApiDetails", new ApiJsonViewModel { Title = "Payment Created", ControllerName = "Payment", Result = result });
    }

    public async Task<IActionResult> Details(int id) => View("ApiDetails", new ApiJsonViewModel { Title = "Payment Details", ControllerName = "Payment", Result = await service.GetAsync(id) });
    public async Task<IActionResult> Dashboard(int? agencyId, int? officeId) => View("ApiDetails", new ApiJsonViewModel { Title = "Payment Dashboard", ControllerName = "Payment", Result = await service.DashboardAsync(agencyId, officeId) });
    public async Task<IActionResult> RevenueSummary(int? agencyId, int? officeId) => View("ApiDetails", new ApiJsonViewModel { Title = "Revenue Summary", ControllerName = "Payment", Result = await service.RevenueSummaryAsync(agencyId, officeId) });
}
