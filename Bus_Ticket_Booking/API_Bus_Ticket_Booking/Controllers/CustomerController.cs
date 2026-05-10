using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.DTOs.Booking;
using API_Bus_Ticket_Booking.DTOs.Customer;
using API_Bus_Ticket_Booking.DTOs.Review;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IBookingService _bookingService;
    private readonly IReviewService _reviewService;

    // private readonly ITripService _tripService;

    // public CustomerController(
    //     ICustomerService customerService,
    //     IBookingService bookingService,
    //     IReviewService reviewService,
    //     ITripService tripService
    // )
    // {
    //     _customerService = customerService;
    //     _bookingService = bookingService;
    //     _reviewService = reviewService;
    //     _tripService = tripService;
    // }

    public CustomerController(
        ICustomerService customerService,
        IBookingService bookingService,
        IReviewService reviewService
    )
    {
        _customerService = customerService;
        _bookingService = bookingService;
        _reviewService = reviewService;
    }

    // ───────────────────────────────────────────────────
    // CUSTOMER PROFILE
    // ───────────────────────────────────────────────────

    // POST /api/customers
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CustomerCreateDto dto)
    {
        if (await _customerService.EmailAlreadyExistsAsync(dto.Email))
            return Conflict(new { message = "A customer with this email already exists." });

        var result = await _customerService.CreateCustomerAsync(dto);
        return CreatedAtAction(nameof(GetProfile), new { customerId = result.CustomerId }, result);
    }

    // GET /api/customers/{customerId}
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetProfile(int customerId)
    {
        var customer = await _customerService.GetCustomerAsync(customerId);
        return customer == null ? NotFound() : Ok(customer);
    }

    // PUT /api/customers/{customerId}
    [HttpPut("{customerId}")]
    public async Task<IActionResult> UpdateProfile(int customerId, [FromBody] CustomerUpdateDto dto)
    {
        var updated = await _customerService.UpdateCustomerAsync(customerId, dto);
        return updated ? NoContent() : NotFound();
    }

    // DELETE /api/customers/{customerId}
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> DeleteAccount(int customerId)
    {
        var deleted = await _customerService.DeleteCustomerAsync(customerId);
        return deleted ? NoContent() : NotFound();
    }

    // ───────────────────────────────────────────────────
    // TRIP SEARCH (customer-facing)
    // ───────────────────────────────────────────────────

    // GET /api/customers/trips/search?fromCity=Delhi&toCity=Mumbai&tripDate=2025-06-01&minSeats=2&maxFare=500
    // [HttpGet("trips/search")]
    // public async Task<IActionResult> SearchTrips([FromQuery] TripSearchDto filters)
    // {
    //     var trips = await _tripService.SearchTripsAsync(filters);
    //     return Ok(trips);
    // }

    // GET /api/customers/trips/upcoming
    // [HttpGet("trips/upcoming")]
    // public async Task<IActionResult> GetUpcomingTrips()
    // {
    //     var trips = await _tripService.GetUpcomingTripsAsync();
    //     return Ok(trips);
    // }

    // GET /api/customers/trips/{tripId}
    // [HttpGet("trips/{tripId}")]
    // public async Task<IActionResult> GetTripDetails(int tripId)
    // {
    //     var trip = await _tripService.GetTripDetailsAsync(tripId);
    //     return trip == null ? NotFound() : Ok(trip);
    // }

    // GET /api/customers/trips/{tripId}/seats
    [HttpGet("trips/{tripId}/seats")]
    public async Task<IActionResult> GetAvailableSeats(int tripId)
    {
        var seats = await _bookingService.GetAvailableSeatsAsync(tripId);
        return Ok(new { tripId, availableSeats = seats });
    }

    // GET /api/customers/trips/route/{routeId}
    // [HttpGet("trips/route/{routeId}")]
    // public async Task<IActionResult> GetTripsByRoute(int routeId)
    // {
    //     var trips = await _tripService.GetTripsByRouteAsync(routeId);
    //     return Ok(trips);
    // }

    // ───────────────────────────────────────────────────
    // BOOKINGS
    // ───────────────────────────────────────────────────

    // POST /api/customers/{customerId}/bookings
    [HttpPost("{customerId}/bookings")]
    public async Task<IActionResult> BookSeat(int customerId, [FromBody] BookSeatRequestDto dto)
    {
        var (success, message, bookingId) = await _bookingService.BookSeatAsync(
            customerId,
            dto.TripId,
            dto.SeatNumber
        );
        if (!success)
            return BadRequest(new { message });
        return Ok(new { bookingId, message });
    }

    // GET /api/customers/{customerId}/bookings
    [HttpGet("{customerId}/bookings")]
    public async Task<IActionResult> GetMyBookings(int customerId)
    {
        var bookings = await _bookingService.GetCustomerBookingsAsync(customerId);
        return Ok(bookings);
    }

    // GET /api/customers/{customerId}/bookings/{bookingId}
    [HttpGet("{customerId}/bookings/{bookingId}")]
    public async Task<IActionResult> GetBookingDetail(int customerId, int bookingId)
    {
        var booking = await _bookingService.GetBookingDetailAsync(customerId, bookingId);
        return booking == null ? NotFound() : Ok(booking);
    }

    // DELETE /api/customers/{customerId}/bookings/{bookingId}
    [HttpDelete("{customerId}/bookings/{bookingId}")]
    public async Task<IActionResult> CancelBooking(int customerId, int bookingId)
    {
        var (success, message) = await _bookingService.CancelBookingAsync(customerId, bookingId);
        return success ? Ok(new { message }) : BadRequest(new { message });
    }

    // ───────────────────────────────────────────────────
    // REVIEWS
    // ───────────────────────────────────────────────────

    // POST /api/customers/{customerId}/reviews
    [HttpPost("{customerId}/reviews")]
    public async Task<IActionResult> AddReview(int customerId, [FromBody] ReviewCreateDto dto)
    {
        var (success, message, review) = await _reviewService.CreateReviewAsync(customerId, dto);
        if (!success)
            return BadRequest(new { message });
        return CreatedAtAction(
            nameof(GetReview),
            new { customerId, reviewId = review!.ReviewId },
            review
        );
    }

    // GET /api/customers/{customerId}/reviews
    [HttpGet("{customerId}/reviews")]
    public async Task<IActionResult> GetMyReviews(int customerId)
    {
        var reviews = await _reviewService.GetCustomerReviewsAsync(customerId);
        return Ok(reviews);
    }

    // GET /api/customers/{customerId}/reviews/{reviewId}
    [HttpGet("{customerId}/reviews/{reviewId}")]
    public async Task<IActionResult> GetReview(int customerId, int reviewId)
    {
        var review = await _reviewService.GetReviewByIdAsync(reviewId);
        if (review == null || review.CustomerId != customerId)
            return NotFound();
        return Ok(review);
    }

    // PUT /api/customers/{customerId}/reviews/{reviewId}
    [HttpPut("{customerId}/reviews/{reviewId}")]
    public async Task<IActionResult> UpdateReview(
        int customerId,
        int reviewId,
        [FromBody] ReviewUpdateDto dto
    )
    {
        var (success, message) = await _reviewService.UpdateReviewAsync(customerId, reviewId, dto);
        return success ? NoContent() : BadRequest(new { message });
    }

    // DELETE /api/customers/{customerId}/reviews/{reviewId}
    [HttpDelete("{customerId}/reviews/{reviewId}")]
    public async Task<IActionResult> DeleteReview(int customerId, int reviewId)
    {
        var (success, message) = await _reviewService.DeleteReviewAsync(customerId, reviewId);
        return success ? NoContent() : NotFound(new { message });
    }

    // GET /api/customers/trips/{tripId}/reviews  → Public: see all reviews for a trip
    [HttpGet("trips/{tripId}/reviews")]
    public async Task<IActionResult> GetTripReviews(int tripId)
    {
        var reviews = await _reviewService.GetTripReviewsAsync(tripId);
        var avg = await _reviewService.GetTripAverageRatingAsync(tripId);
        return Ok(
            new
            {
                tripId,
                averageRating = Math.Round(avg, 1),
                reviews,
            }
        );
    }
}
