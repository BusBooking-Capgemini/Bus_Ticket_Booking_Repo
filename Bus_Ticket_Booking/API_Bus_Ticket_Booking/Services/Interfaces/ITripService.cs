using API_Bus_Ticket_Booking.DTOs.Trip;

namespace API_Bus_Ticket_Booking.Services.Interfaces
{
    public interface ITripService
    {
        Task<List<TripResponseDto>> GetAllTripsAsync();
        Task<TripResponseDto> GetTripByIdAsync(int id);
        Task<List<TripResponseDto>> SearchTripsAsync(TripSearchDto dto);
        Task<List<TripResponseDto>> GetTripsByRouteAsync(int routeId);
        Task<TripSeatMapDto> GetSeatMapAsync(int tripId);
        Task<List<TripResponseDto>> GetTripsByDateAsync(DateTime date);
        Task<List<TripResponseDto>> GetTripsByBusAsync(int busId);
        Task<List<TripResponseDto>> GetTripsByDriverAsync(int driverId);
        Task<List<TripResponseDto>> GetUpcomingTripsAsync();
        Task<TripResponseDto> CreateTripAsync(CreateTripDto dto);
        Task<TripResponseDto> UpdateTripAsync(int id, UpdateTripDto dto);
        Task<bool> DeleteTripAsync(int id);
    }
}
