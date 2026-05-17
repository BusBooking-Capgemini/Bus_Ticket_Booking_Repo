using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IBookingService _bookingService;

        private readonly IPaymentService _paymentService;

        public PaymentController(IBookingService bookingService, IPaymentService paymentService)
        {
            _bookingService = bookingService;

            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var model = new PaymentViewModel
            {
                TripId = Convert.ToInt32(TempData["TripId"]),

                SeatNumber = Convert.ToInt32(TempData["SeatNumber"]),

                Fare = decimal.Parse(TempData["Fare"]!.ToString()!),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(PaymentViewModel model)
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // PROCESS PAYMENT

            bool paymentSuccess = await _paymentService.ProcessPaymentAsync(model);

            if (!paymentSuccess)
            {
                ViewBag.Error = "Payment failed.";

                return View("Checkout", model);
            }

            // CREATE BOOKING

            var booking = await _bookingService.CreateBookingAsync(
                model.TripId,
                model.SeatNumber,
                token
            );

            if (booking == null)
            {
                ViewBag.Error = "Booking failed.";

                return View("Checkout", model);
            }

            // CREATE PAYMENT ENTRY

            bool paymentCreated = await _paymentService.CreatePaymentAsync(
                booking.BookingId,
                model.Fare,
                token
            );

            if (!paymentCreated)
            {
                ViewBag.Error = "Payment entry creation failed.";

                return View("Checkout", model);
            }

            return RedirectToAction("Success", "Booking", new { bookingId = booking.BookingId });
        }

        public async Task<IActionResult> MyPayments()
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var payments = await _paymentService.GetMyPaymentsAsync(token);

            return View(payments);
        }
    }
}
