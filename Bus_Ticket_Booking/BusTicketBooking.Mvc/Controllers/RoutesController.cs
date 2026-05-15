using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class RoutesController(IRouteService service) : Controller
{
    public async Task<IActionResult> Index() => View("ApiTable", new ApiJsonViewModel { Title = "Routes", ControllerName = "Routes", CreateAction = "Create", Result = await service.GetAllAsync() });
    public async Task<IActionResult> Details(int id) => View("ApiDetails", new ApiJsonViewModel { Title = "Route Details", ControllerName = "Routes", Result = await service.GetAsync(id) });
    [HttpGet] public IActionResult Create() => View(new CreateRouteViewModel());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(CreateRouteViewModel model) => await Save(model, service.CreateAsync, nameof(Index));
    [HttpGet] public IActionResult Edit(int id) => View(new CreateRouteViewModel());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, CreateRouteViewModel model) => await Save(model, m => service.UpdateAsync(id, m), nameof(Index));
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { await service.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    private async Task<IActionResult> Save(CreateRouteViewModel model, Func<CreateRouteViewModel, Task<ApiResult<System.Text.Json.JsonElement>>> call, string successAction)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await call(model);
        if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Message ?? "Request failed."); return View(model); }
        return RedirectToAction(successAction);
    }
}
