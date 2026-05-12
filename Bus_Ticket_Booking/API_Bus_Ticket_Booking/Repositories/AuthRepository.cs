using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BusTicketBookingContext _context;

        public AuthRepository(
            BusTicketBookingContext context)
        {
            _context = context;
        }

        // CUSTOMER LOGIN

        public async Task<Customer?> GetCustomerByEmailAsync(
            string email)
        {
            return await _context.Customers
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x =>
                    x.Email == email);
        }

        // CUSTOMER EMAIL CHECK

        public async Task<bool> CustomerEmailExistsAsync(
            string email)
        {
            return await _context.Customers
                .AnyAsync(x =>
                    x.Email == email);
        }

        // CREATE CUSTOMER

        public async Task<Customer> CreateCustomerAsync(
            Customer customer)
        {
            await _context.Customers
                .AddAsync(customer);

            await _context.SaveChangesAsync();

            return customer;
        }

        // AGENCY LOGIN

        public async Task<Agency?> GetAgencyByEmailAsync(
            string email)
        {
            return await _context.Agencies
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x =>
                    x.Email == email);
        }

        // OFFICE LOGIN

        public async Task<AgencyOffice?> GetOfficeByEmailAsync(
            string email)
        {
            return await _context.AgencyOffices
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x =>
                    x.OfficeMail == email);
        }

        // ROLE

        public async Task<Role?> GetRoleByNameAsync(
            string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x =>
                    x.RoleName == roleName);
        }
    }
}