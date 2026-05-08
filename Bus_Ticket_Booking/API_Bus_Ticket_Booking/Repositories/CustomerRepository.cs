using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly BusTicketBookingContext _context;

    public CustomerRepository(BusTicketBookingContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        return await _context
            .Customers.Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context
            .Customers.Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<bool> ExistsAsync(int customerId)
    {
        return await _context.Customers.AnyAsync(c => c.CustomerId == customerId);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Customer customer)
    {
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}
