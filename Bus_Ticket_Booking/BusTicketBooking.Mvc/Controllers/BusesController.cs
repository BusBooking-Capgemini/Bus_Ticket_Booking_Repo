using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class BusesController(IBusService service) : Controller
{
    public async Task<IActionResult> Index() => View("ApiTable", new ApiJsonViewModel { Title = "Buses", ControllerName = "Buses", CreateAction = "Create", Result = await service.GetAllAsync() });
    public async Task<IActionResult> Details(int id) => View("ApiDetails", new ApiJsonViewModel { Title = "Bus Details", ControllerName = "Buses", Result = await service.GetAsync(id) });
    [HttpGet] public IActionResult Create() => View(new BusRequestViewModel());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(BusRequestViewModel model) => await Save(model, service.CreateAsync, nameof(Index));
    [HttpGet] public IActionResult Edit(int id) => View(new BusRequestViewModel());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, BusRequestViewModel model) => await Save(model, m => service.UpdateAsync(id, m), nameof(Index));
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { await service.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    private async Task<IActionResult> Save(BusRequestViewModel model, Func<BusRequestViewModel, Task<ApiResult<System.Text.Json.JsonElement>>> call, string successAction)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await call(model);
        if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Message ?? "Request failed."); return View(model); }
        return RedirectToAction(successAction);
    }
}
