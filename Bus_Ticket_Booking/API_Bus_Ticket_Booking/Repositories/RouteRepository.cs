using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly BusTicketBookingContext _context;

        public RouteRepository(BusTicketBookingContext context)
        {
            _context = context;
        }

        public async Task<List<API_Bus_Ticket_Booking.Models.Route>> GetAllAsync()
        {
            return await _context.Routes.ToListAsync();
        }

        public async Task<API_Bus_Ticket_Booking.Models.Route> GetByIdAsync(int id)
        {
            return await _context.Routes.FirstOrDefaultAsync(r => r.RouteId == id);
        }

        public async Task<List<API_Bus_Ticket_Booking.Models.Route>> SearchAsync(
            string fromCity,
            string toCity
        )
        {
            return await _context
                .Routes.Where(r =>
                    r.FromCity.ToLower().Contains(fromCity.ToLower())
                    && r.ToCity.ToLower().Contains(toCity.ToLower())
                )
                .ToListAsync();
        }

        public async Task<List<API_Bus_Ticket_Booking.Models.Route>> GetByFromCityAsync(string city)
        {
            return await _context
                .Routes.Where(r => r.FromCity.ToLower() == city.ToLower())
                .ToListAsync();
        }

        public async Task<List<API_Bus_Ticket_Booking.Models.Route>> GetByCityAsync(string city)
        {
            return await _context
                .Routes.Where(r => r.ToCity.ToLower() == city.ToLower())
                .ToListAsync();
        }

        public async Task<List<API_Bus_Ticket_Booking.Models.Route>> GetByMaxDurationAsync(
            int maxMinutes
        )
        {
            return await _context
                .Routes.Where(r => r.Duration != null && r.Duration <= maxMinutes)
                .ToListAsync();
        }

        public async Task<List<API_Bus_Ticket_Booking.Models.Trip>> GetTripsByRouteIdAsync(
            int routeId
        )
        {
            return await _context
                .Trips.Include(t => t.Bus)
                .Include(t => t.Driver1Driver)
                .Include(t => t.Driver2Driver)
                .Where(t => t.RouteId == routeId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Routes.AnyAsync(r => r.RouteId == id);
        }

        public async Task<API_Bus_Ticket_Booking.Models.Route> CreateAsync(
            API_Bus_Ticket_Booking.Models.Route route
        )
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<API_Bus_Ticket_Booking.Models.Route> UpdateAsync(
            API_Bus_Ticket_Booking.Models.Route route
        )
        {
            _context.Routes.Update(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task DeleteAsync(int id)
        {
            var route = await _context.Routes.FirstOrDefaultAsync(r => r.RouteId == id);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
            }
        }
    }
}
