using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces
{
    public interface IRouteRepository
    {
        Task<List<API_Bus_Ticket_Booking.Models.Route>> GetAllAsync();
        Task<API_Bus_Ticket_Booking.Models.Route> GetByIdAsync(int id);
        Task<List<API_Bus_Ticket_Booking.Models.Route>> SearchAsync(string fromCity, string toCity);
        Task<List<API_Bus_Ticket_Booking.Models.Route>> GetByFromCityAsync(string city);
        Task<List<API_Bus_Ticket_Booking.Models.Route>> GetByCityAsync(string city);
        Task<List<API_Bus_Ticket_Booking.Models.Route>> GetByMaxDurationAsync(int maxMinutes);
        Task<List<API_Bus_Ticket_Booking.Models.Trip>> GetTripsByRouteIdAsync(int routeId);
        Task<bool> ExistsAsync(int id);
        Task<API_Bus_Ticket_Booking.Models.Route> CreateAsync(API_Bus_Ticket_Booking.Models.Route route);
        Task<API_Bus_Ticket_Booking.Models.Route> UpdateAsync(API_Bus_Ticket_Booking.Models.Route route);
        Task DeleteAsync(int id);
    }
}
