using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces;

public interface IReviewRepository
{
    Task<List<Review>> GetByCustomerIdAsync(int customerId);
    Task<Review?> GetByIdAsync(int reviewId);
    Task<bool> CustomerAlreadyReviewedTripAsync(int customerId, int tripId);
    Task<int> GetNextReviewIdAsync();
    Task<Review> CreateAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Review review);
    Task<List<Review>> GetByTripIdAsync(int tripId);
    Task<double> GetAverageRatingForTripAsync(int tripId);
}