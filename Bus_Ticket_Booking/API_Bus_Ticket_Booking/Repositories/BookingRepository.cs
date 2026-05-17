using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BusTicketBookingContext _context;

        public BookingRepository(BusTicketBookingContext context)
        {
            _context = context;
        }

        public async Task<Booking?> CreateBookingAsync(Booking booking)
        {
            var existingSeat = await _context.Bookings.FirstOrDefaultAsync(b =>
                b.TripId == booking.TripId && b.SeatNumber == booking.SeatNumber
            );

            if (existingSeat == null)
            {
                return null;
            }

            if (existingSeat.Status == "Booked")
            {
                throw new ConflictException("Seat already booked");
            }

            existingSeat.Status = "Booked";

            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.TripId == booking.TripId);

            if (trip != null && trip.AvailableSeats > 0)
            {
                trip.AvailableSeats -= 1;
            }

            await SaveChangesAsync();

            return existingSeat;
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context
                .Bookings.Include(b => b.Trip)
                    .ThenInclude(t => t.Route)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetCustomerBookingsAsync(int customerId)
        {
            return await _context
                .Payments.Include(p => p.Booking)
                    .ThenInclude(b => b!.Trip)
                        .ThenInclude(t => t.Route)
                .Where(p => p.CustomerId == customerId)
                .Select(p => p.Booking!)
                .Where(b => b.Status == "Booked")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetOfficeBookingsAsync(int officeId)
        {
            return await _context
                .Bookings.Include(b => b.Trip)
                    .ThenInclude(t => t.Route)
                .Include(b => b.Payments)
                .Where(b =>
                    b.Status == "Booked"
                    && b.TripId != null
                    && _context.Trips.Any(t =>
                        t.TripId == b.TripId
                        && _context.Buses.Any(bus =>
                            bus.BusId == t.BusId && bus.OfficeId == officeId
                        )
                    )
                )
                .OrderByDescending(b => b.BookingId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAgencyBookingsAsync(int agencyId)
        {
            return await _context
                .Bookings.Include(b => b.Trip)
                    .ThenInclude(t => t.Route)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Bus)
                .Include(b => b.Payments)
                .Where(b =>
                    b.Status == "Booked"
                    && b.Trip != null
                    && b.Trip.Bus != null
                    && b.Trip.Bus.Office != null
                    && b.Trip.Bus.Office.AgencyId == agencyId
                )
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByTripAsync(int tripId)
        {
            return await _context
                .Bookings.Include(b => b.Trip)
                    .ThenInclude(t => t.Route)
                .Include(b => b.Payments)
                .Where(b => b.TripId == tripId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CancelBookingAsync(Booking booking)
        {
            var existingBooking = await _context.Bookings.FirstOrDefaultAsync(b =>
                b.BookingId == booking.BookingId
            );

            if (existingBooking == null)
            {
                return;
            }

            existingBooking.Status = "Available";

            var trip = await _context.Trips.FirstOrDefaultAsync(t =>
                t.TripId == existingBooking.TripId
            );

            if (trip != null)
            {
                trip.AvailableSeats += 1;
            }

            var payments = await _context
                .Payments.Where(p => p.BookingId == existingBooking.BookingId)
                .ToListAsync();

            if (payments.Any())
            {
                _context.Payments.RemoveRange(payments);
            }

            await SaveChangesAsync();
        }

        public async Task<int> GetTotalBookingsByOfficeAsync(int officeId)
        {
            return await _context.Bookings.CountAsync(b =>
                b.Status == "Booked"
                && b.Trip != null
                && b.Trip.Bus != null
                && b.Trip.Bus.OfficeId == officeId
            );
        }

        public async Task<int> GetActiveBookingsByOfficeAsync(int officeId)
        {
            return await GetTotalBookingsByOfficeAsync(officeId);
        }

        public async Task<int> GetTotalBookingsByAgencyAsync(int agencyId)
        {
            return await _context.Bookings.CountAsync(b =>
                b.Status == "Booked"
                && b.Trip != null
                && b.Trip.Bus != null
                && b.Trip.Bus.Office != null
                && b.Trip.Bus.Office.AgencyId == agencyId
            );
        }

        public async Task<int> GetActiveBookingsByAgencyAsync(int agencyId)
        {
            return await GetTotalBookingsByAgencyAsync(agencyId);
        }

        public async Task<double> GetOccupancyRateByOfficeAsync(int officeId)
        {
            var trips = await _context
                .Trips.Where(t => t.Bus != null && t.Bus.OfficeId == officeId)
                .Include(t => t.Bus)
                .AsNoTracking()
                .ToListAsync();

            if (!trips.Any())
            {
                return 0;
            }

            double totalCapacity = trips.Sum(t => t.Bus.Capacity);

            double bookedSeats = trips.Sum(t => t.Bus.Capacity - t.AvailableSeats);

            return (bookedSeats / totalCapacity) * 100;
        }

        public async Task<double> GetOccupancyRateByAgencyAsync(int agencyId)
        {
            var trips = await _context
                .Trips.Where(t =>
                    t.Bus != null && t.Bus.Office != null && t.Bus.Office.AgencyId == agencyId
                )
                .Include(t => t.Bus)
                .AsNoTracking()
                .ToListAsync();

            if (!trips.Any())
            {
                return 0;
            }

            double totalCapacity = trips.Sum(t => t.Bus.Capacity);

            double bookedSeats = trips.Sum(t => t.Bus.Capacity - t.AvailableSeats);

            return (bookedSeats / totalCapacity) * 100;
        }

        public async Task<int> GetMostBookedTripByOfficeAsync(int officeId)
        {
            var result = await _context
                .Bookings.Where(b =>
                    b.Status == "Booked"
                    && b.Trip != null
                    && b.Trip.Bus != null
                    && b.Trip.Bus.OfficeId == officeId
                )
                .GroupBy(b => b.TripId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return result ?? 0;
        }

        public async Task<int> GetMostBookedTripByAgencyAsync(int agencyId)
        {
            var result = await _context
                .Bookings.Where(b =>
                    b.Status == "Booked"
                    && b.Trip != null
                    && b.Trip.Bus != null
                    && b.Trip.Bus.Office != null
                    && b.Trip.Bus.Office.AgencyId == agencyId
                )
                .GroupBy(b => b.TripId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return result ?? 0;
        }

        public async Task<string> GetMostPopularRouteByOfficeAsync(int officeId)
        {
            var result = await _context
                .Bookings.Where(b =>
                    b.Status == "Booked"
                    && b.Trip != null
                    && b.Trip.Route != null
                    && b.Trip.Bus != null
                    && b.Trip.Bus.OfficeId == officeId
                )
                .GroupBy(b => b.Trip!.Route!.FromCity + " - " + b.Trip.Route.ToCity)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return result ?? string.Empty;
        }

        public async Task<string> GetMostPopularRouteByAgencyAsync(int agencyId)
        {
            var result = await _context
                .Bookings.Where(b =>
                    b.Status == "Booked"
                    && b.Trip != null
                    && b.Trip.Route != null
                    && b.Trip.Bus != null
                    && b.Trip.Bus.Office != null
                    && b.Trip.Bus.Office.AgencyId == agencyId
                )
                .GroupBy(b => b.Trip!.Route!.FromCity + " - " + b.Trip.Route.ToCity)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return result ?? string.Empty;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
