using System.Security.Claims;
using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.DTOs.Booking;
using API_Bus_Ticket_Booking.DTOs.Customer;
using API_Bus_Ticket_Booking.DTOs.Review;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    // private readonly IBookingService _bookingService;
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

    public CustomerController(ICustomerService customerService, IReviewService reviewService)
    {
        _customerService = customerService;
        // _bookingService = bookingService;
        _reviewService = reviewService;
    }

    // ───────────────────────────────────────────────────
    // CUSTOMER PROFILE
    // ───────────────────────────────────────────────────

    // POST /api/customers
    // [HttpPost("register")]
    // [AllowAnonymous]

    // public async Task<IActionResult> Register([FromBody] CustomerCreateDto dto)
    // {
    //     if (await _customerService.EmailAlreadyExistsAsync(dto.Email))
    //         return Conflict(new
    //         {
    //             message = "A customer with this email already exists."
    //         });
    //     var result = await _customerService.CreateCustomerAsync(dto);

    //     return CreatedAtAction(nameof(GetProfile), new { customerId = result.CustomerId }, result);
    // }

    // POST /api/customers
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] CustomerRequestDto dto)
    {
        if (await _customerService.EmailAlreadyExistsAsync(dto.Email!))
            return Conflict(new { message = "A customer with this email already exists." });

        var result = await _customerService.CreateCustomerAsync(dto);

        return CreatedAtAction(nameof(GetProfile), new { customerId = result.CustomerId }, result);
    }

    // GET /api/customers
    [HttpGet("getCustomer")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetProfile()
    {
        var customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (customerId <= 0)
            throw new ForbiddenException();

        var customer = await _customerService.GetCustomerAsync(customerId);
        return customer == null ? NotFound() : Ok(customer);
    }

    // PUT /api/customers/updateCustomer/{customerId}
    // [HttpPatch("updateCustomer/{customerId}")]
    // [Authorize(Roles = "Customer")]
    // public async Task<IActionResult> UpdateProfile(int customerId, [FromBody] CustomerUpdateDto dto)
    // {
    //     var updated = await _customerService.UpdateCustomerAsync(customerId, dto);
    //     return updated ? NoContent() : NotFound();
    // }

    // PUT /api/customers/updateCustomer/{customerId}
    [HttpPatch("updateCustomer")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> UpdateProfile([FromBody] CustomerRequestDto dto)
    {
        var customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (customerId <= 0)
            throw new ForbiddenException();

        var updated = await _customerService.UpdateCustomerAsync(customerId, dto);

        return updated ? NoContent() : NotFound();
    }

    // DELETE /api/customers/deleteCustomer
    [HttpDelete("deleteCustomer")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> DeleteAccount()
    {
        var customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (customerId <= 0)
            throw new ForbiddenException();

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
    // ───────────────────────────────────────────────────

    // GET /api/customers/trips/{tripId}
    // [HttpGet("trips/getAvailableSeats/{tripId}")]
    // public async Task<IActionResult> GetAvailableSeats(int tripId)
    // {
    //     var seats = await _bookingService.GetAvailableSeatsAsync(tripId);
    //     return Ok(new { tripId, availableSeats = seats });
    // }

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

    // POST /api/customers/{customerId}/createBooking
    // [HttpPost("{customerId}/createBooking")]
    // public async Task<IActionResult> BookSeat(int customerId, [FromBody] BookSeatRequestDto dto)
    // {
    //     var (success, message, bookingId) = await _bookingService.BookSeatAsync(
    //         customerId,
    //         dto.TripId,
    //         dto.SeatNumber
    //     );
    //     if (!success)
    //         return BadRequest(new { message });
    //     return Ok(new { bookingId, message });
    // }

    // GET /api/customers/{customerId}/getMybookings
    // [HttpGet("{customerId}/getMyBookings")]
    // public async Task<IActionResult> GetMyBookings(int customerId)
    // {
    //     var bookings = await _bookingService.GetCustomerBookingsAsync(customerId);
    //     return Ok(bookings);
    // }

    // GET /api/customers/{customerId}/getBooking/{bookingId}
    // [HttpGet("{customerId}/getBookings/{bookingId}")]
    // public async Task<IActionResult> GetBookingDetail(int customerId, int bookingId)
    // {
    //     var booking = await _bookingService.GetBookingDetailAsync(customerId, bookingId);
    //     return booking == null ? NotFound() : Ok(booking);
    // }

    // DELETE /api/customers/{customerId}/deleteBooking/{bookingId}
    // [HttpDelete("{customerId}/deleteBooking/{bookingId}")]
    // public async Task<IActionResult> CancelBooking(int customerId, int bookingId)
    // {
    //     var (success, message) = await _bookingService.CancelBookingAsync(customerId, bookingId);
    //     return success ? Ok(new { message }) : BadRequest(new { message });
    // }

    // ───────────────────────────────────────────────────
    // REVIEWS
    // ───────────────────────────────────────────────────

    // POST /api/customers/{customerId}/createReview
    // [HttpPost("{customerId}/createReview")]
    // public async Task<IActionResult> AddReview(int customerId, [FromBody] ReviewRequestDto dto)
    // {
    //     var (success, message, review) = await _reviewService.CreateReviewAsync(customerId, dto);
    //     if (!success)
    //         return BadRequest(new { message });
    //     return CreatedAtAction(
    //         nameof(GetReview),
    //         new { customerId, reviewId = review!.ReviewId },
    //         review
    //     );
    // }

    // GET /api/customers/{customerId}/getReviews
    [HttpGet("{customerId}/getReviews")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMyReviews(int customerId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (userId != customerId)
            throw new ForbiddenException("You can only access your own reviews.");

        var reviews = await _reviewService.GetCustomerReviewsAsync(customerId);
        return Ok(reviews);
    }

    // GET /api/customers/{customerId}/getReview/{reviewId}
    [HttpGet("{customerId}/getReview/{reviewId}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetReview(int customerId, int reviewId)
    {
        var review = await _reviewService.GetReviewByIdAsync(reviewId);
        if (review == null || review.CustomerId != customerId)
            return NotFound();
        return Ok(review);
    }

    // PUT /api/customers/{customerId}/updateReview/{reviewId}
    [HttpPatch("{customerId}/updateReview/{reviewId}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> UpdateReview(
        int customerId,
        int reviewId,
        [FromBody] ReviewRequestDto dto
    )
    {
        var (success, message) = await _reviewService.UpdateReviewAsync(customerId, reviewId, dto);
        // return success ? NoContent() : BadRequest(new { message });

        return success ? Ok(new { message }) : BadRequest(new { message });
    }

    // DELETE /api/customers/{customerId}/deleteReview/{reviewId}
    [HttpDelete("{customerId}/deleteReview/{reviewId}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> DeleteReview(int customerId, int reviewId)
    {
        var (success, message) = await _reviewService.DeleteReviewAsync(customerId, reviewId);
        return success ? NoContent() : NotFound(new { message });
    }

    // GET /api/customers/trips/{tripId}/reviews  → Public: see all reviews for a trip
    [HttpGet("trips/{tripId}/getTripReviews")]
    [AllowAnonymous]
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