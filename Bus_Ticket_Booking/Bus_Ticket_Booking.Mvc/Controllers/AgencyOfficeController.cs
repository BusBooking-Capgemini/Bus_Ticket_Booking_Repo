using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class AgencyOfficeController
        : Controller
    {
        private readonly IAgencyDashboardService
            _dashboardService;

        public AgencyOfficeController(
            IAgencyDashboardService dashboardService)
        {
            _dashboardService =
                dashboardService;
        }

        // =========================
        // OFFICE DETAILS
        // =========================

        public async Task<IActionResult>
            Details(
                int officeId)
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
                    .GetOfficeDetailsAsync(
                        agencyId,
                        officeId,
                        token);

            if (model == null)
            {
                return RedirectToAction(
                    "Index",
                    "AgencyDashboard");
            }

            return View(model);
        }
    }
}