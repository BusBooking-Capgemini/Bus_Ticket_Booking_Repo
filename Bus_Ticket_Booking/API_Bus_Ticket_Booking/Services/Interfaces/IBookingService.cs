using API_Bus_Ticket_Booking.DTOs.Booking;

namespace API_Bus_Ticket_Booking.Services.Interfaces;

public interface IBookingService
{
    Task<List<BookingResponseDto>> GetCustomerBookingsAsync(int customerId);
    Task<BookingResponseDto?> GetBookingDetailAsync(int customerId, int bookingId);
    Task<(bool success, string message, int? bookingId)> BookSeatAsync(
        int customerId,
        int tripId,
        int seatNumber
    );
    Task<List<int>> GetAvailableSeatsAsync(int tripId);
    Task<(bool success, string message)> CancelBookingAsync(int customerId, int bookingId);
}
