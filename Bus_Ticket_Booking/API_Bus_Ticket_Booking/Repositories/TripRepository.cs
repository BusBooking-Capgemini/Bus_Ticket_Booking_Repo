using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories;

public class TripRepository : ITripRepository{
    private readonly BusTicketBookingContext _context;

    public TripRepository(BusTicketBookingContext context){
        _context = context;
    }

    public async Task<List<Trip>> SearchTripsAsync(string? fromCity, string? toCity, DateTime? date, int? minSeats, decimal? maxFare){
        // build the query will execute it later.
        // 'AsQueryable' converts the result into a query that can be dynamically modified.
        var query = _context.Trips
            .Include(t => t.Route)
            .Include(t => t.Bus)
            .AsQueryable();

        // All the given parameters are optional, so only add those into our query if they exist.

        if (!string.IsNullOrEmpty(fromCity))
            query = query.Where(t => t.Route.FromCity.ToLower().Contains(fromCity.ToLower()));

        if (!string.IsNullOrEmpty(toCity))
            query = query.Where(t => t.Route.ToCity.ToLower().Contains(toCity.ToLower()));

        if (date.HasValue)
            query = query.Where(t => t.TripDate.Date == date.Value.Date);

        if (minSeats.HasValue)
            query = query.Where(t => t.AvailableSeats >= minSeats.Value);

        if (maxFare.HasValue)
            query = query.Where(t => t.Fare <= maxFare.Value);

        // Only return future trips
        query = query.Where(t => t.TripDate >= DateTime.UtcNow.Date);

        return await query.OrderBy(t => t.TripDate).ToListAsync();
    }

    public async Task<Trip?> GetByIdAsync(int tripId){
        return await _context.Trips
            .Include(t => t.Route)
            .Include(t => t.Bus)
            .Include(t => t.Driver1Driver)
            .Include(t => t.Driver2Driver)
            .FirstOrDefaultAsync(t => t.TripId == tripId);
    }

    public async Task<List<Trip>> GetUpcomingTripsAsync(){
        // 'take' is used to extract a specific number of elements from the beginning of a collection
        return await _context.Trips.Where(t => t.TripDate >= DateTime.UtcNow.Date).Include(t => t.Route).Include(t => t.Bus).OrderBy(t => t.TripDate).Take(20).ToListAsync();
    }

    public async Task<List<Trip>> GetTripsByRouteAsync(int routeId){
        // only return those trips that will come in the future.
        return await _context.Trips
            .Where(t => t.RouteId == routeId && t.TripDate >= DateTime.UtcNow.Date)
            .Include(t => t.Route)
            .Include(t => t.Bus)
            .OrderBy(t => t.TripDate)
            .ToListAsync();
    }
}

