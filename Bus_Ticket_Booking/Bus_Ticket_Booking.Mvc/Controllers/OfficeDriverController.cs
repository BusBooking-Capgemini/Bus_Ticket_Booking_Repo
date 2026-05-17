using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Driver;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class OfficeDriverController : Controller
    {
        private readonly IDriverService
            _driverService;

        public OfficeDriverController(
            IDriverService driverService)
        {
            _driverService =
                driverService;
        }

        // =========================
        // INDEX
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

            var drivers =
                await _driverService
                    .GetOfficeDriversAsync(
                        token);

            return View(drivers);
        }

        // =========================
        // CREATE GET
        // =========================

        [HttpGet]
        public IActionResult Create()
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

            var officeIdClaim =
                JwtHelper.GetClaim(
                    token,
                    "OfficeId");

            int.TryParse(
                officeIdClaim,
                out int officeId);

            var model =
                new CreateDriverViewModel
                {
                    OfficeId =
                        officeId
                };

            return View(model);
        }

        // =========================
        // CREATE POST
        // =========================

        [HttpPost]
        public async Task<IActionResult>
            Create(
                CreateDriverViewModel model)
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

            var officeIdClaim =
                JwtHelper.GetClaim(
                    token,
                    "OfficeId");

            int.TryParse(
                officeIdClaim,
                out int officeId);

            model.OfficeId =
                officeId;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success =
                await _driverService
                    .CreateDriverAsync(
                        model,
                        token);

            if (!success)
            {
                ViewBag.Error =
                    "Driver creation failed.";

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

            var driver =
                await _driverService
                    .GetDriverByIdAsync(
                        id,
                        token);

            if (driver == null)
            {
                return RedirectToAction(
                    "Index");
            }

            var model =
                new UpdateDriverViewModel
                {
                    DriverId =
                        driver.DriverId,

                    LicenseNumber =
                        driver.LicenseNumber,

                    Name =
                        driver.Name,

                    Phone =
                        driver.Phone,

                    OfficeId =
                        driver.OfficeId,

                    Address =
                        driver.Address,

                    City =
                        driver.City,

                    State =
                        driver.State,

                    ZipCode =
                        driver.ZipCode
                };

            return View(model);
        }

        // =========================
        // EDIT POST
        // =========================

        [HttpPost]
        public async Task<IActionResult>
            Edit(
                UpdateDriverViewModel model)
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

            var officeIdClaim =
                JwtHelper.GetClaim(
                    token,
                    "OfficeId");

            int.TryParse(
                officeIdClaim,
                out int officeId);

            model.OfficeId =
                officeId;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success =
                await _driverService
                    .UpdateDriverAsync(
                        model,
                        token);

            if (!success)
            {
                ViewBag.Error =
                    "Driver update failed.";

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

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "OfficeLogin",
                    "Auth");
            }

            await _driverService
                .DeleteDriverAsync(
                    id,
                    token);

            return RedirectToAction(
                "Index");
        }
    }
}