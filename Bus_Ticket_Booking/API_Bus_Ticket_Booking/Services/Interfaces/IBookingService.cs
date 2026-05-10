using API_Bus_Ticket_Booking.DTOs.Booking;

namespace API_Bus_Ticket_Booking.Services.Interfaces;

public interface IBookingService
{
    Task<List<BookingResponseForCustomerDto>> GetCustomerBookingsForCustomerControllerAsync(
        int customerId
    );
    Task<BookingResponseForCustomerDto?> GetBookingDetailAsync(int customerId, int bookingId);
    Task<(bool success, string message, int? bookingId)> BookSeatAsync(
        int customerId,
        int tripId,
        int seatNumber
    );
    Task<List<int>> GetAvailableSeatsAsync(int tripId);
    Task<(bool success, string message)> CancelBookingAsync(int customerId, int bookingId);

    Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto);

    Task CancelBookingAsync(int bookingId);

    Task<BookingResponseDto> GetBookingByIdAsync(int bookingId);

    Task<IEnumerable<BookingResponseDto>> GetCustomerBookingsAsync(int customerId);

    Task<IEnumerable<BookingResponseDto>> GetOfficeBookingsAsync(int officeId);

    Task<IEnumerable<BookingResponseDto>> GetAgencyBookingsAsync(int agencyId);

    Task<BookingDashboardDto> GetDashboardAsync(int agencyId, int? officeId);

    Task<BookingAnalyticsDto> GetAnalyticsAsync(int agencyId, int? officeId);
}
