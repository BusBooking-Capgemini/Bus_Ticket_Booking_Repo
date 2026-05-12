using API_Bus_Ticket_Booking.DTOs.Booking;

namespace API_Bus_Ticket_Booking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateBookingAsync(
            CreateBookingDto dto);

        Task CancelBookingAsync(
            int bookingId);

        Task<BookingResponseDto> GetBookingByIdAsync(
            int bookingId);

        Task<IEnumerable<BookingResponseDto>>
            GetCustomerBookingsAsync(
                int customerId);

        Task<IEnumerable<BookingResponseDto>>
            GetOfficeBookingsAsync(
                int officeId);

        Task<IEnumerable<BookingResponseDto>>
            GetAgencyBookingsAsync(
                int agencyId);

        Task<IEnumerable<BookingResponseDto>>
            GetBookingsByTripAsync(
                int tripId);

        Task<BookingDashboardDto>
            GetDashboardAsync(
                int agencyId,
                int? officeId);

        Task<BookingAnalyticsDto>
            GetAnalyticsAsync(
                int agencyId,
                int? officeId);
    }
}