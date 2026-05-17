using API_Bus_Ticket_Booking.DTOs.Review;

namespace API_Bus_Ticket_Booking.Services.Interfaces;

public interface IReviewService
{
    Task<List<ReviewResponseDto>> GetCustomerReviewsAsync(int customerId);
    Task<ReviewResponseDto?> GetReviewByIdAsync(int reviewId);

    Task<(bool success, string message, ReviewResponseDto? review)> CreateReviewAsync(
        int customerId,
        ReviewRequestDto dto
    );

    Task<(bool success, string message)> UpdateReviewAsync(
        int customerId,
        int reviewId,
        ReviewRequestDto dto
    );

    Task<(bool success, string message)> DeleteReviewAsync(int customerId, int reviewId);
    Task<List<ReviewResponseDto>> GetTripReviewsAsync(int tripId);
    Task<double> GetTripAverageRatingAsync(int tripId);
}
