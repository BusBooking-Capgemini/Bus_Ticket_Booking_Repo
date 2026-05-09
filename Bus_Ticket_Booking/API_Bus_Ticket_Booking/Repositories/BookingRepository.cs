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

        // Create Booking
        public async Task<Booking?> CreateBookingAsync(Booking booking)
        {
            var existingSeat = await _context.Bookings
                .FirstOrDefaultAsync(b =>
                    b.TripId == booking.TripId &&
                    b.SeatNumber == booking.SeatNumber);

            if (existingSeat == null)
            {
                return null;
            }

            if (existingSeat.Status == "Booked")
            {
                throw new ConflictException("Seat already booked");
            }

            existingSeat.Status = "Booked";

            var trip = await _context.Trips
                .FirstOrDefaultAsync(t => t.TripId == booking.TripId);

            if (trip != null && trip.AvailableSeats > 0)
            {
                trip.AvailableSeats -= 1;
            }

            _context.Bookings.Update(existingSeat);

            await SaveChangesAsync();

            return existingSeat;
        }

        // Get Booking By Id
        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Trip)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        // Customer Bookings
        public async Task<IEnumerable<Booking>> GetCustomerBookingsAsync(int customerId)
        {
            return await _context.Payments
                .Include(p => p.Booking)
                .ThenInclude(b => b.Trip)
                .Where(p => p.CustomerId == customerId)
                .Select(p => p.Booking)
                .Where(b => b.Status == "Booked")
                .ToListAsync();
        }

        // Office Bookings
        public async Task<IEnumerable<Booking>> GetOfficeBookingsAsync(int officeId)
        {
            return await _context.Bookings
                .Include(b => b.Trip)
                .ThenInclude(t => t.Bus)
                .Where(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus.OfficeId == officeId)
                .ToListAsync();
        }

        // Agency Bookings
        public async Task<IEnumerable<Booking>> GetAgencyBookingsAsync(int agencyId)
        {
            return await _context.Bookings
                .Include(b => b.Trip)
                .ThenInclude(t => t.Bus)
                .ThenInclude(bus => bus.Office)
                .Where(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus.Office.AgencyId == agencyId)
                .ToListAsync();
        }

        // Trip Bookings
        public async Task<IEnumerable<Booking>> GetBookingsByTripAsync(int tripId)
        {
            return await _context.Bookings
                .Where(b =>
                    b.Status == "Booked" &&
                    b.TripId == tripId)
                .ToListAsync();
        }

        // Cancel Booking
        public async Task CancelBookingAsync(Booking booking)
        {
            booking.Status = "Available";

            var trip = await _context.Trips
                .FirstOrDefaultAsync(t => t.TripId == booking.TripId);

            if (trip != null)
            {
                trip.AvailableSeats += 1;
            }

            _context.Bookings.Update(booking);

            await SaveChangesAsync();
        }

        // Dashboard - Office

        public async Task<int> GetTotalBookingsByOfficeAsync(int officeId)
        {
            return await _context.Bookings
                .CountAsync(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus.OfficeId == officeId);
        }

        public async Task<int> GetActiveBookingsByOfficeAsync(int officeId)
        {
            return await _context.Bookings
                .CountAsync(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus.OfficeId == officeId);
        }

        public async Task<int> GetCancelledBookingsByOfficeAsync(int officeId)
        {
            return 0;
        }

        public async Task<int> GetCompletedBookingsByOfficeAsync(int officeId)
        {
            return 0;
        }

        // Dashboard - Agency

        public async Task<int> GetTotalBookingsByAgencyAsync(int agencyId)
        {
            return await _context.Bookings
                .CountAsync(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus.Office.AgencyId == agencyId);
        }

        public async Task<int> GetActiveBookingsByAgencyAsync(int agencyId)
        {
            return await _context.Bookings
                .CountAsync(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus.Office.AgencyId == agencyId);
        }

        public async Task<int> GetCancelledBookingsByAgencyAsync(int agencyId)
        {
            return 0;
        }

        public async Task<int> GetCompletedBookingsByAgencyAsync(int agencyId)
        {
            return 0;
        }

        // Analytics

        public async Task<double> GetOccupancyRateByOfficeAsync(int officeId)
        {
            var trips = await _context.Trips
                .Include(t => t.Bus)
                .Where(t => t.Bus.OfficeId == officeId)
                .ToListAsync();

            if (!trips.Any())
            {
                return 0;
            }

            double totalCapacity = trips.Sum(t => t.Bus.Capacity);

            double bookedSeats = trips.Sum(t =>
                t.Bus.Capacity - t.AvailableSeats);

            return (bookedSeats / totalCapacity) * 100;
        }

        public async Task<double> GetOccupancyRateByAgencyAsync(int agencyId)
        {
            var trips = await _context.Trips
                .Include(t => t.Bus)
                .Where(t => t.Bus.Office.AgencyId == agencyId)
                .ToListAsync();

            if (!trips.Any())
            {
                return 0;
            }

            double totalCapacity = trips.Sum(t => t.Bus.Capacity);

            double bookedSeats = trips.Sum(t =>
                t.Bus.Capacity - t.AvailableSeats);

            return (bookedSeats / totalCapacity) * 100;
        }

        public async Task<int> GetMostBookedTripByOfficeAsync(int officeId)
        {
            var result = await _context.Bookings
                .Where(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus.OfficeId == officeId)
                .GroupBy(b => b.TripId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return result ?? 0;
        }

        public async Task<int> GetMostBookedTripByAgencyAsync(int agencyId)
        {
            var result = await _context.Bookings
                .Where(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus.Office.AgencyId == agencyId)
                .GroupBy(b => b.TripId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return result ?? 0;
        }

        public async Task<string> GetMostPopularRouteByOfficeAsync(int officeId)
        {
            var result = await _context.Bookings
                .Where(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus.OfficeId == officeId)
                .GroupBy(b =>
                    b.Trip.Route.FromCity + " - " + b.Trip.Route.ToCity)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return result ?? string.Empty;
        }

        public async Task<string> GetMostPopularRouteByAgencyAsync(int agencyId)
        {
            var result = await _context.Bookings
                .Where(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus.Office.AgencyId == agencyId)
                .GroupBy(b =>
                    b.Trip.Route.FromCity + " - " + b.Trip.Route.ToCity)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return result ?? string.Empty;
        }

        // Save Changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}