using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class OfficeBusController : Controller
    {
        private readonly IBusService
            _busService;

        public OfficeBusController(
            IBusService busService)
        {
            _busService =
                busService;
        }

        // =========================
        // ALL BUSES
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

            var buses =
                await _busService
                    .GetOfficeBusesAsync(token);

            return View(buses);
        }

        // =========================
        // CREATE GET
        // =========================

        [HttpGet]
        public IActionResult Create()
        {
            var model =
                new CreateBusViewModel
                {
                    BusTypes =
                    [
                        new SelectListItem
                {
                    Value = "Seater",
                    Text = "Seater"
                },

                new SelectListItem
                {
                    Value = "AC Seater",
                    Text = "AC Seater"
                },

                new SelectListItem
                {
                    Value = "Sleeper",
                    Text = "Sleeper"
                },

                new SelectListItem
                {
                    Value = "AC Sleeper",
                    Text = "AC Sleeper"
                },

                new SelectListItem
                {
                    Value = "Semi-Sleeper",
                    Text = "Semi-Sleeper"
                }
                    ]
                };

            return View(model);
        }

        // =========================
        // CREATE POST
        // =========================

        [HttpPost]
        public async Task<IActionResult>
    Create(
        CreateBusViewModel model)
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

            model.BusTypes =
            [
                new SelectListItem
        {
            Value = "Seater",
            Text = "Seater"
        },

        new SelectListItem
        {
            Value = "AC Seater",
            Text = "AC Seater"
        },

        new SelectListItem
        {
            Value = "Sleeper",
            Text = "Sleeper"
        },

        new SelectListItem
        {
            Value = "AC Sleeper",
            Text = "AC Sleeper"
        },

        new SelectListItem
        {
            Value = "Semi-Sleeper",
            Text = "Semi-Sleeper"
        }
            ];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var officeIdClaim =
                JwtHelper.GetClaim(
                    token,
                    "OfficeId");

            if (string.IsNullOrEmpty(
                officeIdClaim))
            {
                ViewBag.Error =
                    "Office ID not found in token.";

                return View(model);
            }

            model.OfficeId =
                Convert.ToInt32(
                    officeIdClaim);

            var success =
                await _busService
                    .CreateBusAsync(
                        model,
                        token);

            if (!success)
            {
                ViewBag.Error =
                    "Bus creation failed.";

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

            var bus =
                await _busService
                    .GetBusByIdAsync(
                        id,
                        token);

            if (bus == null)
            {
                ViewBag.Error =
                    "Bus not found.";

                return RedirectToAction(
                    "Index");
            }

            var model =
                new UpdateBusViewModel
                {
                    BusId =
                        bus.BusId,

                    RegistrationNumber =
                        bus.RegistrationNumber,

                    Capacity =
                        bus.Capacity,

                    Type =
                        bus.Type,

                    OfficeId =
            bus.OfficeId,

                    BusTypes =
                    [
                        new SelectListItem
                {
                    Value = "Seater",
                    Text = "Seater"
                },

                new SelectListItem
                {
                    Value = "AC Seater",
                    Text = "AC Seater"
                },

                new SelectListItem
                {
                    Value = "Sleeper",
                    Text = "Sleeper"
                },

                new SelectListItem
                {
                    Value = "AC Sleeper",
                    Text = "AC Sleeper"
                },

                new SelectListItem
                {
                    Value = "Semi-Sleeper",
                    Text = "Semi-Sleeper"
                }
                    ]
                };

            return View(model);
        }

        // =========================
        // EDIT POST
        // =========================

        [HttpPost]
        public async Task<IActionResult>
    Edit(
        UpdateBusViewModel model)
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

            model.BusTypes =
            [
                new SelectListItem
        {
            Value = "Seater",
            Text = "Seater"
        },

        new SelectListItem
        {
            Value = "AC Seater",
            Text = "AC Seater"
        },

        new SelectListItem
        {
            Value = "Sleeper",
            Text = "Sleeper"
        },

        new SelectListItem
        {
            Value = "AC Sleeper",
            Text = "AC Sleeper"
        },

        new SelectListItem
        {
            Value = "Semi-Sleeper",
            Text = "Semi-Sleeper"
        }
            ];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var officeIdClaim =
    JwtHelper.GetClaim(
        token,
        "OfficeId");

            model.OfficeId =
                Convert.ToInt32(
                    officeIdClaim);

            var success =
                await _busService
                    .UpdateBusAsync(
                        model,
                        token);

            if (!success)
            {
                ViewBag.Error =
                    "Bus update failed.";

                return View(model);
            }

            return RedirectToAction(
                "Index");
        }

        // =========================
        // DELETE
        // =========================

        public async Task<IActionResult>
            Delete(int id)
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            await _busService
                .DeleteBusAsync(
                    id,
                    token!);

            return RedirectToAction(
                "Index");
        }
    }
}