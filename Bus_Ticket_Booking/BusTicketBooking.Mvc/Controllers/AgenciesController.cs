using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class AgenciesController(IAgencyService service) : Controller
{
    public async Task<IActionResult> Index() => View("ApiTable", new ApiJsonViewModel { Title = "Agencies", ControllerName = "Agencies", Result = await service.GetAllAsync() });
    public async Task<IActionResult> Details(int id) => View("ApiDetails", new ApiJsonViewModel { Title = "Agency Details", ControllerName = "Agencies", Result = await service.GetAsync(id) });
    public async Task<IActionResult> Offices(int id) => View("ApiTable", new ApiJsonViewModel { Title = "Agency Offices", ControllerName = "Agencies", Result = await service.GetOfficesAsync(id) });
    public async Task<IActionResult> Summary(int id) => View("ApiDetails", new ApiJsonViewModel { Title = "Agency Summary", ControllerName = "Agencies", Result = await service.GetSummaryAsync(id) });

    [HttpGet]
    public IActionResult Edit(int id) => View(new AgencyRequestViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AgencyRequestViewModel model)
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
