using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces
{
    public interface ITripRepository
    {
        Task<List<Trip>> GetAllAsync();
        Task<Trip?> GetByIdAsync(int id);
        Task<List<Trip>> SearchAsync(string? fromCity,string? toCity,DateTime? tripDate);
        Task<List<Trip>> GetByRouteIdAsync(int routeId);
        Task<List<Trip>> GetByDateAsync(DateTime date);
        Task<List<Trip>> GetByBusIdAsync(int busId);
        Task<List<Trip>> GetByDriverIdAsync(int driverId);
        Task<List<Trip>> GetUpcomingAsync();

        Task CreateSeatEntriesAsync(List<Booking> bookings);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsBusAvailableAsync(int busId, DateTime tripDate, DateTime departure, DateTime arrival, int excludeTripId);
        Task<bool> IsDriverAvailableAsync(int driverId, DateTime tripDate, DateTime departure, DateTime arrival, int excludeTripId);
        Task<List<Booking>> GetBookingsByTripIdAsync(int tripId);
        Task<int> GetBusCapacityAsync(int busId);
        Task<Trip> CreateAsync(Trip trip);
        Task<Trip> UpdateAsync(Trip trip);
        Task DeleteAsync(int id);

        Task<List<Trip>> GetByOfficeIdAsync(int officeId);

        Task<List<Trip>> GetByAgencyIdAsync(int agencyId);
    }
}
