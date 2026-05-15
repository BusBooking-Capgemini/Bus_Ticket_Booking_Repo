using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class CustomersController(ICustomerService service) : Controller
{
    public async Task<IActionResult> Profile() => View("ApiDetails", new ApiJsonViewModel { Title = "Customer Profile", ControllerName = "Customers", Result = await service.GetCurrentAsync() });

    [HttpGet]
    public IActionResult Register() => View(new CustomerRequestViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(CustomerRequestViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await service.RegisterAsync(model);
        if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed."); return View(model); }
        return RedirectToAction(nameof(Profile));
    }

    [HttpGet]
    public IActionResult Edit() => View(new CustomerRequestViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CustomerRequestViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await service.UpdateCurrentAsync(model);
        if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Message ?? "Update failed."); return View(model); }
        return RedirectToAction(nameof(Profile));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete()
    {
        await service.DeleteCurrentAsync();
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");
    }
}
