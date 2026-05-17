using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class OfficeDashboardController
        : Controller
    {
        private readonly IOfficeDashboardService
            _dashboardService;

        public OfficeDashboardController(
            IOfficeDashboardService dashboardService)
        {
            _dashboardService =
                dashboardService;
        }


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

            var officeIdString =
                JwtHelper.GetOfficeId(token);

            if (string.IsNullOrEmpty(officeIdString))
            {
                return RedirectToAction(
                    "OfficeLogin",
                    "Auth");
            }

            int officeId =
                Convert.ToInt32(
                    officeIdString);

            var dashboard =
                await _dashboardService
                    .GetDashboardAsync(
                        officeId,
                        token);

            return View(dashboard);
        }
    }
}