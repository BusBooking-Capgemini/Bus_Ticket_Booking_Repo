using Bus_Ticket_Booking.Mvc.ViewModels.Review;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewViewModel>> GetMyReviewsAsync(int customerId, string token);

        Task<bool> CreateReviewAsync(int customerId, CreateReviewViewModel model, string token);

        Task<bool> DeleteReviewAsync(int customerId, int reviewId, string token);
    }
}
