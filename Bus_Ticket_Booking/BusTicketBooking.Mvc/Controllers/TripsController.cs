using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.Helpers;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class TripsController(
    ITripService service,
    IRouteService routeService,
    IBusService busService,
    IDriverService driverService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var result = await service.GetAllAsync();
        return View("TripList", await BuildTripListAsync("Trips", result));
    }

    public async Task<IActionResult> Details(int id) => View("ApiDetails", new ApiJsonViewModel { Title = "Trip Details", ControllerName = "Trips", Result = await service.GetAsync(id) });

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateTripLookupsAsync();
        return View(new CreateTripViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTripViewModel model) => await Save(model, service.CreateAsync, nameof(Index));

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        await PopulateTripLookupsAsync();
        return View(new CreateTripViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateTripViewModel model) => await Save(model, m => service.UpdateAsync(id, m), nameof(Index));

    [HttpGet]
    public IActionResult Search() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Search(string fromCity, string toCity, DateTime tripDate)
    {
        var result = await service.SearchAsync(fromCity, toCity, tripDate);
        return View("TripList", await BuildTripListAsync("Trip Search Results", result));
    }

    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { await service.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
    private async Task<IActionResult> Save(CreateTripViewModel model, Func<CreateTripViewModel, Task<ApiResult<System.Text.Json.JsonElement>>> call, string successAction)
    {
        if (!ModelState.IsValid)
        {
            await PopulateTripLookupsAsync();
            return View(model);
        }

        var result = await call(model);
        if (!result.Succeeded)
        {
            await PopulateTripLookupsAsync();
            ModelState.AddModelError(string.Empty, result.Message ?? "Request failed.");
            return View(model);
        }

        return RedirectToAction(successAction);
    }

    private async Task PopulateTripLookupsAsync()
    {
        var routes = await routeService.GetAllAsync();
        var buses = await busService.GetAllAsync();
        var drivers = await driverService.GetAllAsync();

        ViewBag.Routes = routes.Succeeded ? JsonLookupHelper.BuildSelectList(routes.Data, "route") : Enumerable.Empty<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
        ViewBag.Buses = buses.Succeeded ? JsonLookupHelper.BuildSelectList(buses.Data, "bus") : Enumerable.Empty<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
        ViewBag.Drivers = drivers.Succeeded ? JsonLookupHelper.BuildSelectList(drivers.Data, "driver") : Enumerable.Empty<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
    }

    private async Task<TripListViewModel> BuildTripListAsync(string title, ApiResult<System.Text.Json.JsonElement> result)
    {
        var routes = await routeService.GetAllAsync();
        var buses = await busService.GetAllAsync();
        var drivers = await driverService.GetAllAsync();

        var routeLookup = routes.Succeeded ? JsonLookupHelper.BuildLookup(routes.Data, "route") : new Dictionary<int, string>();
        var busLookup = buses.Succeeded ? JsonLookupHelper.BuildLookup(buses.Data, "bus") : new Dictionary<int, string>();
        var driverLookup = drivers.Succeeded ? JsonLookupHelper.BuildLookup(drivers.Data, "driver") : new Dictionary<int, string>();

        return new TripListViewModel
        {
            Title = title,
            Result = result,
            Trips = result.Succeeded ? TripDisplayHelper.BuildRows(result.Data, routeLookup, busLookup, driverLookup) : Array.Empty<TripRowViewModel>()
        };
    }
}
