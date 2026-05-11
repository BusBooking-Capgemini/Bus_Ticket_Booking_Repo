using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories
{
    public class AgencyRepository : IAgencyRepository
    {
        private readonly BusTicketBookingContext _context;

        public AgencyRepository(BusTicketBookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agency>> GetAllAsync()
        {
            return await _context.Agencies.ToListAsync();
        }

        public async Task<Agency> GetByIdAsync(int id)
        {
            return await _context.Agencies
                .FirstOrDefaultAsync(x => x.AgencyId == id);
        }

        public async Task UpdateAsync(Agency agency)
        {
            _context.Agencies.Update(agency);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Agency agency)
        {
            _context.Agencies.Remove(agency);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AgencyOffice>> GetAgencyOfficesAsync(int agencyId)
        {
            return await _context.AgencyOffices
                .Where(x => x.AgencyId == agencyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetOfficeBookingsAsync(int agencyId, int officeId)
        {
            return await _context.Bookings
                .Where(x =>
                    x.Trip.Bus.OfficeId == officeId &&
                    x.Trip.Bus.Office.AgencyId == agencyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetOfficePaymentsAsync(int agencyId, int officeId)
        {
            return await _context.Payments
                .Where(x =>
                    x.Booking.Trip.Bus.OfficeId == officeId &&
                    x.Booking.Trip.Bus.Office.AgencyId == agencyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetOfficeTripsAsync(int agencyId, int officeId)
        {
            return await _context.Trips
                .Where(x =>
                    x.Bus.OfficeId == officeId &&
                    x.Bus.Office.AgencyId == agencyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bus>> GetOfficeBusesAsync(int agencyId, int officeId)
        {
            return await _context.Buses
                .Where(x =>
                    x.OfficeId == officeId &&
                    x.Office.AgencyId == agencyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetOfficeDriversAsync(int agencyId, int officeId)
        {
            return await _context.Drivers
                .Where(x =>
                    x.OfficeId == officeId &&
                    x.Office.AgencyId == agencyId)
                .ToListAsync();
        }
    }
}
