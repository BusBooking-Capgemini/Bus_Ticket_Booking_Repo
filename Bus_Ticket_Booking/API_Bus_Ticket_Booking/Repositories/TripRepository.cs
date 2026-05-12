using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly BusTicketBookingContext _context;

        public TripRepository(BusTicketBookingContext context)
        {
            _context = context;
        }

        public async Task<List<Trip>> GetAllAsync()
        {
            // Lazy loading handles navigation properties automatically
            return await _context.Trips.ToListAsync();
        }

        public async Task<Trip> GetByIdAsync(int id)
        {
            return await _context.Trips.FirstOrDefaultAsync(t => t.TripId == id);
        }

        public async Task<List<Trip>> SearchAsync(string fromCity, string toCity, DateTime tripDate)
        {
            // Route nav property accessed via lazy loading
            return await _context.Trips
                .Where(t =>
                    t.Route.FromCity.ToLower() == fromCity.ToLower() &&
                    t.Route.ToCity.ToLower() == toCity.ToLower() &&
                    t.TripDate.Date == tripDate.Date)
                .ToListAsync();
        }

        public async Task<List<Trip>> GetByRouteIdAsync(int routeId)
        {
            return await _context.Trips
                .Where(t => t.RouteId == routeId)
                .ToListAsync();
        }

        public async Task<List<Trip>> GetByDateAsync(DateTime date)
        {
            return await _context.Trips
                .Where(t => t.TripDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task<List<Trip>> GetByBusIdAsync(int busId)
        {
            return await _context.Trips
                .Where(t => t.BusId == busId)
                .ToListAsync();
        }

        public async Task<List<Trip>> GetByDriverIdAsync(int driverId)
        {
            return await _context.Trips
                .Where(t => t.Driver1DriverId == driverId || t.Driver2DriverId == driverId)
                .ToListAsync();
        }

        public async Task<List<Trip>> GetUpcomingAsync()
        {
            DateTime today = DateTime.Today;
            return await _context.Trips
                .Where(t => t.TripDate >= today)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Trips.AnyAsync(t => t.TripId == id);
        }

        public async Task<bool> IsBusAvailableAsync(
            int busId,
            DateTime tripDate,
            DateTime departure,
            DateTime arrival,
            int excludeTripId)
        {
            bool hasConflict = await _context.Trips.AnyAsync(t =>
                t.BusId == busId &&
                t.TripId != excludeTripId &&
                t.TripDate.Date == tripDate.Date &&
                t.DepartureTime < arrival &&
                t.ArrivalTime > departure);

            return !hasConflict;
        }

        public async Task<bool> IsDriverAvailableAsync(
            int driverId,
            DateTime tripDate,
            DateTime departure,
            DateTime arrival,
            int excludeTripId)
        {
            bool hasConflict = await _context.Trips.AnyAsync(t =>
                (t.Driver1DriverId == driverId || t.Driver2DriverId == driverId) &&
                t.TripId != excludeTripId &&
                t.TripDate.Date == tripDate.Date &&
                t.DepartureTime < arrival &&
                t.ArrivalTime > departure);

            return !hasConflict;
        }

        public async Task<List<Booking>> GetBookingsByTripIdAsync(int tripId)
        {
            return await _context.Bookings
                .Where(b => b.TripId == tripId)
                .ToListAsync();
        }

        public async Task<int> GetBusCapacityAsync(int busId)
        {
            var bus = await _context.Buses.FirstOrDefaultAsync(b => b.BusId == busId);
            if (bus == null)
            {
                return 0;
            }
            return bus.Capacity;
        }

        public async Task<Trip> CreateAsync(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
            return trip;
        }

        public async Task<Trip> UpdateAsync(Trip trip)
        {
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
            return trip;
        }

        public async Task DeleteAsync(int id)
        {
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.TripId == id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
            }
        }
    }
}
