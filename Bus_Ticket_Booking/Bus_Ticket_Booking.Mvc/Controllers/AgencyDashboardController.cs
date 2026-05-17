using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class AgencyDashboardController
        : Controller
    {
        private readonly IAgencyDashboardService
            _dashboardService;

        public AgencyDashboardController(
            IAgencyDashboardService dashboardService)
        {
            _dashboardService =
                dashboardService;
        }

        // =========================
        // AGENCY DASHBOARD
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
                    "AgencyLogin",
                    "Auth");
            }

            var agencyIdString =
                JwtHelper.GetAgencyId(
                    token);

            if (string.IsNullOrEmpty(
                agencyIdString))
            {
                return RedirectToAction(
                    "AgencyLogin",
                    "Auth");
            }

            int agencyId =
                Convert.ToInt32(
                    agencyIdString);

            var model =
                await _dashboardService
                    .GetDashboardAsync(
                        agencyId,
                        token);

            return View(model);
        }
    }
}