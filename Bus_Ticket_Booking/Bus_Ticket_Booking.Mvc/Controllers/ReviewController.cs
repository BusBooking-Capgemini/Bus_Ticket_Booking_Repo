using Bus_Ticket_Booking.Mvc.Filters;
using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    [CustomerAuthorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IBookingService _bookingService;

        public ReviewController(IReviewService reviewService, IBookingService bookingService)
        {
            _reviewService = reviewService;
            _bookingService = bookingService;
        }

        // GET /Review/MyReviews
        [HttpGet]
        public async Task<IActionResult> MyReviews()
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token);
            var userId = HttpContext.Session.GetString(SessionKeys.UserId);

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            int customerId = int.Parse(userId);

            var reviews = await _reviewService.GetMyReviewsAsync(customerId, token);

            return View(reviews);
        }

        // GET /Review/Create?tripId=5&fromCity=Delhi&toCity=Mumbai
        [HttpGet]
        public IActionResult Create(int tripId, string fromCity, string toCity)
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var model = new CreateReviewViewModel
            {
                TripId = tripId,
                FromCity = fromCity,
                ToCity = toCity,
            };

            return View(model);
        }

        // POST /Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewViewModel model)
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token);
            var userId = HttpContext.Session.GetString(SessionKeys.UserId);

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(model);

            int customerId = int.Parse(userId);

            var success = await _reviewService.CreateReviewAsync(customerId, model, token);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "Could not submit review. You may have already reviewed this trip, or you have not booked it."
                );
                return View(model);
            }

            TempData["SuccessMessage"] = "Your review was submitted successfully!";

            return RedirectToAction(nameof(MyReviews));
        }

        // POST /Review/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int reviewId)
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token);
            var userId = HttpContext.Session.GetString(SessionKeys.UserId);

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            int customerId = int.Parse(userId);

            await _reviewService.DeleteReviewAsync(customerId, reviewId, token);

            TempData["SuccessMessage"] = "Review deleted successfully.";

            return RedirectToAction(nameof(MyReviews));
        }
    }
}