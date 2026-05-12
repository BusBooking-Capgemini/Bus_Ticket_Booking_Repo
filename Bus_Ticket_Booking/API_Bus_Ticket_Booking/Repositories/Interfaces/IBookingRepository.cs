using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        // Create Booking
        Task<Booking?> CreateBookingAsync(
            Booking booking);

        // Get Booking By Id
        Task<Booking?> GetBookingByIdAsync(
            int bookingId);

        // Customer Bookings
        Task<IEnumerable<Booking>>
            GetCustomerBookingsAsync(
                int customerId);

        // Office Bookings
        Task<IEnumerable<Booking>>
            GetOfficeBookingsAsync(
                int officeId);

        // Agency Bookings
        Task<IEnumerable<Booking>>
            GetAgencyBookingsAsync(
                int agencyId);

        // Trip Bookings
        Task<IEnumerable<Booking>>
            GetBookingsByTripAsync(
                int tripId);

        // Cancel Booking
        Task CancelBookingAsync(
            Booking booking);

        // Dashboard - Office
        Task<int> GetTotalBookingsByOfficeAsync(
            int officeId);

        Task<int> GetActiveBookingsByOfficeAsync(
            int officeId);

        // Dashboard - Agency
        Task<int> GetTotalBookingsByAgencyAsync(
            int agencyId);

        Task<int> GetActiveBookingsByAgencyAsync(
            int agencyId);

        // Analytics
        Task<double> GetOccupancyRateByOfficeAsync(
            int officeId);

        Task<double> GetOccupancyRateByAgencyAsync(
            int agencyId);

        Task<int> GetMostBookedTripByOfficeAsync(
            int officeId);

        Task<int> GetMostBookedTripByAgencyAsync(
            int agencyId);

        Task<string> GetMostPopularRouteByOfficeAsync(
            int officeId);

        Task<string> GetMostPopularRouteByAgencyAsync(
            int agencyId);

        // Save Changes
        Task SaveChangesAsync();
    }
}