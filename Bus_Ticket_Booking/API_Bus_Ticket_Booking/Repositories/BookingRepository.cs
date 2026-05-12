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

        public BookingRepository(
            BusTicketBookingContext context)
        {
            _context = context;
        }

        // Create Booking
        public async Task<Booking?> CreateBookingAsync(
            Booking booking)
        {
            var existingSeat =
                await _context.Bookings
                    .FirstOrDefaultAsync(b =>
                        b.TripId == booking.TripId &&
                        b.SeatNumber == booking.SeatNumber);

            if (existingSeat == null)
            {
                return null;
            }

            if (existingSeat.Status == "Booked")
            {
                throw new ConflictException(
                    "Seat already booked");
            }

            existingSeat.Status = "Booked";

            var trip =
                await _context.Trips
                    .FirstOrDefaultAsync(t =>
                        t.TripId == booking.TripId);

            if (trip != null &&
                trip.AvailableSeats > 0)
            {
                trip.AvailableSeats -= 1;
            }

            _context.Bookings.Update(existingSeat);

            await SaveChangesAsync();

            return existingSeat;
        }

        // Get Booking By Id
        public async Task<Booking?> GetBookingByIdAsync(
            int bookingId)
        {
            return await _context.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(b =>
                    b.BookingId == bookingId);
        }

        // Customer Bookings
        public async Task<IEnumerable<Booking>>
            GetCustomerBookingsAsync(
                int customerId)
        {
            return await _context.Payments
                .Where(p =>
                    p.CustomerId == customerId)
                .Select(p => p.Booking!)
                .Where(b =>
                    b.Status == "Booked")
                .AsNoTracking()
                .ToListAsync();
        }

        // Office Bookings
        public async Task<IEnumerable<Booking>>
            GetOfficeBookingsAsync(
                int officeId)
        {
            return await _context.Bookings
                .Where(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus != null &&
                    b.Trip.Bus.OfficeId == officeId)
                .AsNoTracking()
                .ToListAsync();
        }

        // Agency Bookings
        public async Task<IEnumerable<Booking>>
            GetAgencyBookingsAsync(
                int agencyId)
        {
            return await _context.Bookings
                .Where(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus != null &&
                    b.Trip.Bus.Office != null &&
                    b.Trip.Bus.Office.AgencyId == agencyId)
                .AsNoTracking()
                .ToListAsync();
        }

        // Trip Bookings
        public async Task<IEnumerable<Booking>>
            GetBookingsByTripAsync(
                int tripId)
        {
            return await _context.Bookings
                .Where(b =>
                    b.TripId == tripId)
                .AsNoTracking()
                .ToListAsync();
        }

        // Cancel Booking
        public async Task CancelBookingAsync(
            Booking booking)
        {
            booking.Status = "Available";

            var trip =
                await _context.Trips
                    .FirstOrDefaultAsync(t =>
                        t.TripId == booking.TripId);

            if (trip != null)
            {
                trip.AvailableSeats += 1;
            }

            _context.Bookings.Update(booking);

            await SaveChangesAsync();
        }

        // Dashboard - Office
        public async Task<int>
            GetTotalBookingsByOfficeAsync(
                int officeId)
        {
            return await _context.Bookings
                .CountAsync(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus != null &&
                    b.Trip.Bus.OfficeId == officeId);
        }

        public async Task<int>
            GetActiveBookingsByOfficeAsync(
                int officeId)
        {
            return await
                GetTotalBookingsByOfficeAsync(
                    officeId);
        }

        // Dashboard - Agency
        public async Task<int>
            GetTotalBookingsByAgencyAsync(
                int agencyId)
        {
            return await _context.Bookings
                .CountAsync(b =>
                    b.Status == "Booked" &&
                    b.Trip != null &&
                    b.Trip.Bus != null &&
                    b.Trip.Bus.Office != null &&
                    b.Trip.Bus.Office.AgencyId == agencyId);
        }

        public async Task<int>
            GetActiveBookingsByAgencyAsync(
                int agencyId)
        {
            return await
                GetTotalBookingsByAgencyAsync(
                    agencyId);
        }

        // Analytics
        public async Task<double>
            GetOccupancyRateByOfficeAsync(
                int officeId)
        {
            var trips =
                await _context.Trips
                    .Where(t =>
                        t.Bus != null &&
                        t.Bus.OfficeId == officeId)
                    .AsNoTracking()
                    .ToListAsync();

            if (!trips.Any())
            {
                return 0;
            }

            double totalCapacity =
                trips.Sum(t =>
                    t.Bus!.Capacity);

            double bookedSeats =
                trips.Sum(t =>
                    t.Bus!.Capacity -
                    t.AvailableSeats);

            return
                (bookedSeats / totalCapacity) * 100;
        }

        public async Task<double>
            GetOccupancyRateByAgencyAsync(
                int agencyId)
        {
            var trips =
                await _context.Trips
                    .Where(t =>
                        t.Bus != null &&
                        t.Bus.Office != null &&
                        t.Bus.Office.AgencyId == agencyId)
                    .AsNoTracking()
                    .ToListAsync();

            if (!trips.Any())
            {
                return 0;
            }

            double totalCapacity =
                trips.Sum(t =>
                    t.Bus!.Capacity);

            double bookedSeats =
                trips.Sum(t =>
                    t.Bus!.Capacity -
                    t.AvailableSeats);

            return
                (bookedSeats / totalCapacity) * 100;
        }

        public async Task<int>
            GetMostBookedTripByOfficeAsync(
                int officeId)
        {
            var result =
                await _context.Bookings
                    .Where(b =>
                        b.Status == "Booked" &&
                        b.Trip != null &&
                        b.Trip.Bus != null &&
                        b.Trip.Bus.OfficeId == officeId)
                    .GroupBy(b => b.TripId)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync();

            return result ?? 0;
        }

        public async Task<int>
            GetMostBookedTripByAgencyAsync(
                int agencyId)
        {
            var result =
                await _context.Bookings
                    .Where(b =>
                        b.Status == "Booked" &&
                        b.Trip != null &&
                        b.Trip.Bus != null &&
                        b.Trip.Bus.Office != null &&
                        b.Trip.Bus.Office.AgencyId == agencyId)
                    .GroupBy(b => b.TripId)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync();

            return result ?? 0;
        }

        public async Task<string>
            GetMostPopularRouteByOfficeAsync(
                int officeId)
        {
            var result =
                await _context.Bookings
                    .Where(b =>
                        b.Status == "Booked" &&
                        b.Trip != null &&
                        b.Trip.Route != null &&
                        b.Trip.Bus != null &&
                        b.Trip.Bus.OfficeId == officeId)
                    .GroupBy(b =>
                        b.Trip!.Route!.FromCity +
                        " - " +
                        b.Trip.Route.ToCity)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync();

            return result ?? string.Empty;
        }

        public async Task<string>
            GetMostPopularRouteByAgencyAsync(
                int agencyId)
        {
            var result =
                await _context.Bookings
                    .Where(b =>
                        b.Status == "Booked" &&
                        b.Trip != null &&
                        b.Trip.Route != null &&
                        b.Trip.Bus != null &&
                        b.Trip.Bus.Office != null &&
                        b.Trip.Bus.Office.AgencyId == agencyId)
                    .GroupBy(b =>
                        b.Trip!.Route!.FromCity +
                        " - " +
                        b.Trip.Route.ToCity)
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