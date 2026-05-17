using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class OfficeBookingController
        : Controller
    {
        private readonly IOfficeBookingService
            _officeBookingService;

        public OfficeBookingController(
            IOfficeBookingService officeBookingService)
        {
            _officeBookingService =
                officeBookingService;
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

            var bookings =
                await _officeBookingService
                    .GetOfficeBookingsAsync(
                        token);

            return View(bookings);
        }
    }
}