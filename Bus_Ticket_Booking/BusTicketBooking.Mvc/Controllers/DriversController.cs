using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class DriversController(IDriverService service) : Controller
{
    public async Task<IActionResult> Index() => View("ApiTable", new ApiJsonViewModel { Title = "Drivers", ControllerName = "Drivers", CreateAction = "Create", Result = await service.GetAllAsync() });
    public async Task<IActionResult> Details(int id) => View("ApiDetails", new ApiJsonViewModel { Title = "Driver Details", ControllerName = "Drivers", Result = await service.GetAsync(id) });
    [HttpGet] public IActionResult Create() => View(new DriverRequestViewModel());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(DriverRequestViewModel model) => await Save(model, service.CreateAsync, nameof(Index));
    [HttpGet] public IActionResult Edit(int id) => View(new DriverRequestViewModel());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, DriverRequestViewModel model) => await Save(model, m => service.UpdateAsync(id, m), nameof(Index));
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { await service.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    private async Task<IActionResult> Save(DriverRequestViewModel model, Func<DriverRequestViewModel, Task<ApiResult<System.Text.Json.JsonElement>>> call, string successAction)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await call(model);
        if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Message ?? "Request failed."); return View(model); }
        return RedirectToAction(successAction);
    }
}
