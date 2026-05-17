using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Booking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService
            _bookingService;
        private readonly ITripService
            _tripService;

        public BookingController(
            IBookingService bookingService, ITripService tripService)
        {
            _bookingService =
                bookingService;
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(
    int tripId,
    int seatNumber)
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            var trip =
                await _tripService
                    .GetTripByIdAsync(tripId);

            if (trip == null)
            {
                return RedirectToAction(
                    "Index",
                    "Trip");
            }

            var model =
                new BookingCreateViewModel
                {
                    TripId = tripId,
                    SeatNumber = seatNumber,
                    Fare = trip.Fare
                };

            return View(model);
        }


        [HttpPost]
        public IActionResult Create(
            BookingCreateViewModel model)
        {
            TempData["TripId"] =
                model.TripId;

            TempData["SeatNumber"] =
                model.SeatNumber;

            TempData["Fare"] = model.Fare.ToString();

            return RedirectToAction(
                "Checkout",
                "Payment");
        }


        public IActionResult Success(
            int bookingId)
        {
            ViewBag.BookingId =
                bookingId;

            return View();
        }

        public async Task<IActionResult>
            MyBookings()
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            var bookings =
                await _bookingService
                    .GetMyBookingsAsync(token);

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult>
    Cancel(int bookingId)
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            await _bookingService
                .CancelBookingAsync(
                    bookingId,
                    token);

            return RedirectToAction(
                nameof(MyBookings));
        }
    }
}