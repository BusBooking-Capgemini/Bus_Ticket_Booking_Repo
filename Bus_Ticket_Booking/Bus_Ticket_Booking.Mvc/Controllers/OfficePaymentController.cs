using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class OfficePaymentController
        : Controller
    {
        private readonly IOfficePaymentService
            _officePaymentService;

        public OfficePaymentController(
            IOfficePaymentService officePaymentService)
        {
            _officePaymentService =
                officePaymentService;
        }

        public async Task<IActionResult>
            Index()
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(
                token))
            {
                return RedirectToAction(
                    "OfficeLogin",
                    "Auth");
            }

            var payments =
                await _officePaymentService
                    .GetOfficePaymentsAsync(
                        token);

            return View(payments);
        }
    }
}