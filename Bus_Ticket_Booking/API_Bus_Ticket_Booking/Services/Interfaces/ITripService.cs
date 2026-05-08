using API_Bus_Ticket_Booking.DTOs.Trip;

namespace API_Bus_Ticket_Booking.Services.Interfaces;

public interface ITripService
{
    Task<List<TripResponseDto>> SearchTripsAsync(TripSearchDto filters);
    Task<TripResponseDto?> GetTripDetailsAsync(int tripId);
    Task<List<TripResponseDto>> GetUpcomingTripsAsync();
    Task<List<TripResponseDto>> GetTripsByRouteAsync(int routeId);
}
