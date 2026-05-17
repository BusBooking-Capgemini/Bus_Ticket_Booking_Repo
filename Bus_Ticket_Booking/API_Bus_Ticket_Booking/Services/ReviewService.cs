using API_Bus_Ticket_Booking.DTOs.Review;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;

namespace API_Bus_Ticket_Booking.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepo;
    private readonly IBookingRepository _bookingRepo;
    private readonly ICustomerRepository _customerRepo;

    public ReviewService(
        IReviewRepository reviewRepo,
        IBookingRepository bookingRepo,
        ICustomerRepository customerRepo
    )
    {
        _reviewRepo = reviewRepo;
        _bookingRepo = bookingRepo;
        _customerRepo = customerRepo;
    }

    public async Task<List<ReviewResponseDto>> GetCustomerReviewsAsync(int customerId)
    {
        var reviews = await _reviewRepo.GetByCustomerIdAsync(customerId);
        return reviews.Select(MapToDto).ToList();
    }

    public async Task<ReviewResponseDto?> GetReviewByIdAsync(int reviewId)
    {
        var review = await _reviewRepo.GetByIdAsync(reviewId);

        if (review == null)
            throw new NotFoundException("Review not found.");

        return MapToDto(review);
    }

    public async Task<(bool success, string message, ReviewResponseDto? review)> CreateReviewAsync(
        int customerId,
        ReviewRequestDto dto
    )
    {
        // Validate customer exists
        if (!await _customerRepo.ExistsAsync(customerId))
            throw new NotFoundException("Customer not found.");

        // TripId is required for creation
        if (dto.TripId == null || dto.TripId <= 0)
            throw new ValidationException("TripId is required.");

        // Rating is required for creation
        if (dto.Rating == null || dto.Rating < 1 || dto.Rating > 5)
            throw new ValidationException("Rating is required and must be between 1 and 5.");

        int tripId = dto.TripId.Value;
        int rating = dto.Rating.Value;

        // Customer must have booked the trip to leave a review
        if (!await _bookingRepo.CustomerHasBookedTripAsync(customerId, tripId))
            throw new ForbiddenException("You can only review trips you have booked.");

        // Prevent duplicate reviews for the same trip
        if (await _reviewRepo.CustomerAlreadyReviewedTripAsync(customerId, tripId))
            throw new ConflictException("You have already reviewed this trip.");

        var review = new Review
        {
            ReviewId = await _reviewRepo.GetNextReviewIdAsync(),
            CustomerId = customerId,
            TripId = tripId,
            Rating = rating,
            Comment = dto.Comment,
            ReviewDate = DateTime.UtcNow,
        };

        var created = await _reviewRepo.CreateAsync(review);

        // Fetch with navigation properties for full response
        var result = await _reviewRepo.GetByIdAsync(created.ReviewId);

        return (true, "Review submitted successfully.", MapToDto(result!));
    }

    public async Task<(bool success, string message)> UpdateReviewAsync(
        int customerId,
        int reviewId,
        ReviewRequestDto dto
    )
    {
        var review = await _reviewRepo.GetByIdAsync(reviewId);

        if (review == null || review.CustomerId != customerId)
            throw new NotFoundException("Review not found.");

        if (dto.Rating.HasValue && dto.Rating != 0 && dto.Rating != review.Rating)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ValidationException("Rating must be between 1 and 5.");

            review.Rating = dto.Rating.Value;
        }

        if (dto.Comment != null)
            review.Comment = dto.Comment;

        await _reviewRepo.UpdateAsync(review);

        return (true, "Review updated successfully.");
    }

    public async Task<(bool success, string message)> DeleteReviewAsync(
        int customerId,
        int reviewId
    )
    {
        var review = await _reviewRepo.GetByIdAsync(reviewId);

        if (review == null || review.CustomerId != customerId)
            throw new NotFoundException("Review not found.");

        await _reviewRepo.DeleteAsync(review);

        return (true, "Review deleted successfully.");
    }

    public async Task<List<ReviewResponseDto>> GetTripReviewsAsync(int tripId)
    {
        var reviews = await _reviewRepo.GetByTripIdAsync(tripId);
        return reviews.Select(MapToDto).ToList();
    }

    public async Task<double> GetTripAverageRatingAsync(int tripId)
    {
        return await _reviewRepo.GetAverageRatingForTripAsync(tripId);
    }

    private static ReviewResponseDto MapToDto(Review r) =>
        new()
        {
            ReviewId = r.ReviewId,
            CustomerId = r.CustomerId,
            CustomerName = r.Customer?.Name ?? "",
            TripId = r.TripId,
            FromCity = r.Trip?.Route?.FromCity ?? "",
            ToCity = r.Trip?.Route?.ToCity ?? "",
            Rating = r.Rating,
            Comment = r.Comment,
            ReviewDate = r.ReviewDate,
        };
}
