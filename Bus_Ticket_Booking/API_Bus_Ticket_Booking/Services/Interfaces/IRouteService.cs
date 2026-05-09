using API_Bus_Ticket_Booking.DTOs.Route;
using API_Bus_Ticket_Booking.DTOs.Trip;

namespace API_Bus_Ticket_Booking.Services.Interfaces
{
    public interface IRouteService
    {
        Task<List<RouteresponseDto>> GetAllRoutesAsync();
        Task<RouteresponseDto> GetRouteByIdAsync(int id);
        Task<List<RouteresponseDto>> SearchRoutesAsync(string fromCity, string toCity);
        Task<List<TripResponseDto>> GetTripsByRouteAsync(int routeId);
        Task<List<string>> GetAllCitiesAsync();
        Task<List<RouteresponseDto>> GetRoutesByFromCityAsync(string city);
        Task<List<RouteresponseDto>> GetRoutesToCityAsync(string city);
        Task<List<RouteresponseDto>> GetRoutesByMaxDurationAsync(int maxMinutes);
        Task<RouteresponseDto> CreateRouteAsync(CreateRouteDto dto);
        Task<RouteresponseDto> UpdateRouteAsync(int id, UpdateRouteDto dto);
        Task<bool> DeleteRouteAsync(int id);
    }
}
