using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class OfficesController(IOfficeService service) : Controller
{
    public async Task<IActionResult> Index() => View("ApiTable", new ApiJsonViewModel { Title = "Offices", ControllerName = "Offices", CreateAction = "Create", Result = await service.GetAllAsync() });
    public async Task<IActionResult> Details(int id) => View("ApiDetails", new ApiJsonViewModel { Title = "Office Details", ControllerName = "Offices", Result = await service.GetAsync(id) });
    public async Task<IActionResult> Summary(int id) => View("ApiDetails", new ApiJsonViewModel { Title = "Office Summary", ControllerName = "Offices", Result = await service.GetSummaryAsync(id) });

    [HttpGet]
    public IActionResult Create() => View(new OfficeRequestViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OfficeRequestViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await service.CreateAsync(model);
        if (!result.Succeeded) ModelState.AddModelError(string.Empty, result.Message ?? "Create failed.");
        return result.Succeeded ? RedirectToAction(nameof(Index)) : View(model);
    }

    [HttpGet]
    public IActionResult Edit(int id) => View(new OfficeRequestViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, OfficeRequestViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await service.UpdateAsync(id, model);
        if (!result.Succeeded) ModelState.AddModelError(string.Empty, result.Message ?? "Update failed.");
        return result.Succeeded ? RedirectToAction(nameof(Details), new { id }) : View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
