using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Trip;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class OfficeTripController : Controller
    {
        private readonly ITripService
            _tripService;

        public OfficeTripController(
            ITripService tripService)
        {
            _tripService =
                tripService;
        }

        // =========================
        // ALL TRIPS
        // =========================

        public async Task<IActionResult>
            Index()
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "OfficeLogin",
                    "Auth");
            }

            var trips =
                await _tripService
                    .GetOfficeTripsAsync(token);

            return View(trips);
        }

        // =========================
        // CREATE GET
        // =========================

        [HttpGet]
        public async Task<IActionResult>
            Create()
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "OfficeLogin",
                    "Auth");
            }

            var model =
                new CreateTripViewModel();

            model.Routes =
                await _tripService
                    .GetRoutesDropdownAsync(token);

            model.Buses =
                await _tripService
                    .GetBusesDropdownAsync(token);

            model.Addresses =
                await _tripService
                    .GetAddressesDropdownAsync(token);

            model.Drivers =
                await _tripService
                    .GetDriversDropdownAsync(token);

            return View(model);
        }

        // =========================
        // CREATE POST
        // =========================

        [HttpPost]
        public async Task<IActionResult>
            Create(
                CreateTripViewModel model)
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "OfficeLogin",
                    "Auth");
            }

            if (!ModelState.IsValid)
            {
                model.Routes =
                    await _tripService
                        .GetRoutesDropdownAsync(token);

                model.Buses =
                    await _tripService
                        .GetBusesDropdownAsync(token);

                model.Addresses =
                    await _tripService
                        .GetAddressesDropdownAsync(token);

                model.Drivers =
                    await _tripService
                        .GetDriversDropdownAsync(token);

                return View(model);
            }

            var success =
                await _tripService
                    .CreateTripAsync(
                        model,
                        token);

            if (!success)
            {
                ViewBag.Error =
                    "Trip creation failed.";

                model.Routes =
                    await _tripService
                        .GetRoutesDropdownAsync(token);

                model.Buses =
                    await _tripService
                        .GetBusesDropdownAsync(token);

                model.Addresses =
                    await _tripService
                        .GetAddressesDropdownAsync(token);

                model.Drivers =
                    await _tripService
                        .GetDriversDropdownAsync(token);

                return View(model);
            }

            return RedirectToAction(
                "Index");
        }

        // =========================
        // EDIT GET
        // =========================

        [HttpGet]
        public async Task<IActionResult>
    Edit(int id)
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "OfficeLogin",
                    "Auth");
            }

            var trip =
                await _tripService
                    .GetTripByIdAsync(id);

            if (trip == null)
            {
                return RedirectToAction(
                    "Index");
            }

            var model =
    new UpdateTripViewModel
    {
        TripId = trip.TripId,

        BusId = trip.BusId,

        BoardingAddressId =
            trip.BoardingAddressId,

        DroppingAddressId =
            trip.DroppingAddressId,

        Driver1DriverId =
            trip.Driver1DriverId,

        Driver2DriverId =
            trip.Driver2DriverId,

        Fare = trip.Fare,

        TripDate = trip.TripDate,

        DepartureTime =
            trip.DepartureTime,

        ArrivalTime =
            trip.ArrivalTime
    };

            model.Buses =
                await _tripService
                    .GetBusesDropdownAsync(token);

            model.Addresses =
                await _tripService
                    .GetAddressesDropdownAsync(token);

            model.Drivers =
                await _tripService
                    .GetDriversDropdownAsync(token);

            return View(model);
        }

        // =========================
        // EDIT POST
        // =========================

        [HttpPost]
        public async Task<IActionResult>
            Edit(
                UpdateTripViewModel model)
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "OfficeLogin",
                    "Auth");
            }

            if (!ModelState.IsValid)
            {
                model.Buses =
                    await _tripService
                        .GetBusesDropdownAsync(token);

                model.Addresses =
                    await _tripService
                        .GetAddressesDropdownAsync(token);

                model.Drivers =
                    await _tripService
                        .GetDriversDropdownAsync(token);

                return View(model);
            }

            var success =
                await _tripService
                    .UpdateTripAsync(
                        model,
                        token);

            if (!success)
            {
                ViewBag.Error =
                    "Trip update failed.";

                model.Buses =
                    await _tripService
                        .GetBusesDropdownAsync(token);

                model.Addresses =
                    await _tripService
                        .GetAddressesDropdownAsync(token);

                model.Drivers =
                    await _tripService
                        .GetDriversDropdownAsync(token);

                return View(model);
            }

            return RedirectToAction(
                "Index");
        }

        // =========================
        // DELETE
        // =========================

        public async Task<IActionResult>
            Delete(
                int id)
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "OfficeLogin",
                    "Auth");
            }

            await _tripService
                .DeleteTripAsync(
                    id,
                    token);

            return RedirectToAction(
                "Index");
        }
    }
}