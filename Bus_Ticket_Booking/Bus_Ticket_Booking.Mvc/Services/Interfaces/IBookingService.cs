using Bus_Ticket_Booking.Mvc.ViewModels.Booking;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseViewModel?>
            CreateBookingAsync(
                int tripId,
                int seatNumber,
                string token);

        Task<List<BookingListViewModel>>
            GetMyBookingsAsync(
                string token);

        Task<bool>
            CancelBookingAsync(
                int bookingId,
                string token);
    }
}