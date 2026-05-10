using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces;

public interface ITripRepository
{
    Task<List<Trip>> SearchTripsAsync(
        string? fromCity,
        string? toCity,
        DateTime? date,
        int? minSeats,
        decimal? maxFare
    );
    Task<Trip?> GetByIdAsync(int tripId);
    Task<List<Trip>> GetUpcomingTripsAsync();
    Task<List<Trip>> GetTripsByRouteAsync(int routeId);
}
