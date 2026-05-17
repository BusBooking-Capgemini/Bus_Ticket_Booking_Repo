using System.Security.Claims;
using API_Bus_Ticket_Booking.DTOs.Customer;
using API_Bus_Ticket_Booking.DTOs.Review;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IReviewService _reviewService;

    public CustomerController(ICustomerService customerService, IReviewService reviewService)
    {
        _customerService = customerService;
        _reviewService = reviewService;
    }

    // ───────────────────────────────────────────────────
    // CUSTOMER PROFILE
    // ───────────────────────────────────────────────────

    // POST /api/customers/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] CustomerRequestDto dto)
    {
        if (await _customerService.EmailAlreadyExistsAsync(dto.Email!))
            return Conflict(new { message = "A customer with this email already exists." });

        var result = await _customerService.CreateCustomerAsync(dto);

        return CreatedAtAction(nameof(GetProfile), new { customerId = result.CustomerId }, result);
    }

    // GET /api/customers/getCustomer
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

    // PATCH /api/customers/updateCustomer
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
    // REVIEWS
    // ───────────────────────────────────────────────────

    // POST /api/customers/{customerId}/createReview
    [HttpPost("{customerId}/createReview")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> AddReview(int customerId, [FromBody] ReviewRequestDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (userId != customerId)
            throw new ForbiddenException("You can only submit reviews for your own account.");

        var (success, message, review) = await _reviewService.CreateReviewAsync(customerId, dto);

        if (!success)
            return BadRequest(new { message });

        return CreatedAtAction(
            nameof(GetReview),
            new { customerId, reviewId = review!.ReviewId },
            new { message, data = review }
        );
    }

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

    // PATCH /api/customers/{customerId}/updateReview/{reviewId}
    [HttpPatch("{customerId}/updateReview/{reviewId}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> UpdateReview(
        int customerId,
        int reviewId,
        [FromBody] ReviewRequestDto dto
    )
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (userId != customerId)
            throw new ForbiddenException("You can only update your own reviews.");

        var (success, message) = await _reviewService.UpdateReviewAsync(customerId, reviewId, dto);

        return success ? Ok(new { message }) : BadRequest(new { message });
    }

    // DELETE /api/customers/{customerId}/deleteReview/{reviewId}
    [HttpDelete("{customerId}/deleteReview/{reviewId}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> DeleteReview(int customerId, int reviewId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (userId != customerId)
            throw new ForbiddenException("You can only delete your own reviews.");

        var (success, message) = await _reviewService.DeleteReviewAsync(customerId, reviewId);
        return success ? NoContent() : NotFound(new { message });
    }

    // GET /api/customers/trips/{tripId}/getTripReviews  → Public
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
