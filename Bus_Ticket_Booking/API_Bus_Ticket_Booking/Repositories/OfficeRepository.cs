using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Data;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly BusTicketBookingContext _context;

        public OfficeRepository(BusTicketBookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AgencyOffice>> GetAllAsync()
        {
            return await _context.AgencyOffices.ToListAsync();
        }

        public async Task<AgencyOffice> GetByIdAsync(int id)
        {
            return await _context.AgencyOffices
                .FirstOrDefaultAsync(x => x.OfficeId == id);
        }

        public async Task AddAsync(AgencyOffice office)
        {
            await _context.AgencyOffices.AddAsync(office);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AgencyOffice office)
        {
            _context.AgencyOffices.Update(office);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AgencyOffice office)
        {
            _context.AgencyOffices.Remove(office);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Bus>> GetBusesAsync(int officeId)
        {
            return await _context.Buses
                .Where(x => x.OfficeId == officeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetDriversAsync(int officeId)
        {
            return await _context.Drivers
                .Where(x => x.OfficeId == officeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetTripsAsync(int officeId)
        {
            return await _context.Trips
                .Where(x => x.Bus.OfficeId == officeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsAsync(int officeId)
        {
            return await _context.Bookings
                .Where(x => x.Trip.Bus.OfficeId == officeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsAsync(int officeId)
        {
            return await _context.Payments
                .Where(x => x.Booking.Trip.Bus.OfficeId == officeId)
                .ToListAsync();
        }

    }
}
