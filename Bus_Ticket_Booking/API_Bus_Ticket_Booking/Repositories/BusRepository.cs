using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories;

public class BusRepository : IBusRepository
{
    private readonly BusTicketBookingContext _context;

    public BusRepository(BusTicketBookingContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Bus>> GetAllAsync()
    {
        return await _context.Buses.Include(b => b.Office).ToListAsync();
    }

    public async Task<Bus?> GetByIdAsync(int id)
    {
        return await _context.Buses.Include(b => b.Office).FirstOrDefaultAsync(b => b.BusId == id);
    }

    public async Task<IEnumerable<Bus>> GetByOfficeIdAsync(int officeId)
    {
        return await _context
            .Buses.Include(b => b.Office)
            .Where(b => b.OfficeId == officeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Bus>> GetByTypeAsync(string type)
    {
        return await _context
            .Buses.Include(b => b.Office)
            .Where(b => b.Type.ToLower() == type.ToLower())
            .ToListAsync();
    }

    public async Task<Bus?> GetByRegistrationNumberAsync(string regNumber)
    {
        return await _context
            .Buses.Include(b => b.Office)
            .FirstOrDefaultAsync(b => b.RegistrationNumber == regNumber);
    }

    public async Task<IEnumerable<Bus>> GetByCapacityRangeAsync(int min, int max)
    {
        return await _context
            .Buses.Include(b => b.Office)
            .Where(b => b.Capacity >= min && b.Capacity <= max)
            .ToListAsync();
    }

    public async Task<Bus> CreateAsync(Bus bus)
    {
        _context.Buses.Add(bus);
        await _context.SaveChangesAsync();
        return bus;
    }

    public async Task<Bus?> UpdateAsync(int id, Bus bus)
    {
        var existing = await _context.Buses.FindAsync(id);
        if (existing == null)
            return null;

        existing.OfficeId = bus.OfficeId;
        existing.RegistrationNumber = bus.RegistrationNumber;
        existing.Capacity = bus.Capacity;
        existing.Type = bus.Type;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var bus = await _context.Buses.FindAsync(id);
        if (bus == null)
            return false;

        _context.Buses.Remove(bus);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Buses.AnyAsync(b => b.BusId == id);
    }

    public async Task<int> GetTotalBusCountByOfficeAsync(int officeId)
    {
        return await _context.Buses.CountAsync(b => b.OfficeId == officeId);
    }
}
