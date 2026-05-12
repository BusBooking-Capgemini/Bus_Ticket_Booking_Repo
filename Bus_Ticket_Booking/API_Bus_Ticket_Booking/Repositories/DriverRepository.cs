using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly BusTicketBookingContext _context;

    public DriverRepository(BusTicketBookingContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Driver>> GetAllAsync()
    {
        return await _context.Drivers.ToListAsync();
    }

    public async Task<Driver?> GetByIdAsync(int id)
    {
        return await _context.Drivers.FirstOrDefaultAsync(d => d.DriverId == id);
    }

    public async Task<IEnumerable<Driver>> GetByOfficeIdAsync(int officeId)
    {
        return await _context.Drivers
            .Where(d => d.OfficeId == officeId)
            .ToListAsync();
    }

    public async Task<Driver?> GetByLicenseNumberAsync(string licenseNumber)
    {
        return await _context.Drivers
            .FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber);
    }

    public async Task<IEnumerable<Driver>> GetByNameAsync(string name)
    {
        return await _context.Drivers
            .Where(d => d.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();
    }

    public async Task<IEnumerable<Driver>> GetByCityAsync(string city)
    {
        return await _context.Drivers
            .Where(d => d.Address != null && d.Address.City.ToLower() == city.ToLower())
            .ToListAsync();
    }

    public async Task<Driver> CreateAsync(Driver driver)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();
        return driver;
    }

    public async Task<Driver?> UpdateAsync(int id, Driver driver)
    {
        var existing = await _context
            .Drivers.Include(d => d.Address)
            .FirstOrDefaultAsync(d => d.DriverId == id);

        if (existing == null)
            return null;

        existing.LicenseNumber = driver.LicenseNumber;
        existing.Name = driver.Name;
        existing.Phone = driver.Phone;
        existing.OfficeId = driver.OfficeId;

        if (existing.Address != null)
        {
            existing.Address.Address1 = driver.Address?.Address1 ?? existing.Address.Address1;
            existing.Address.City = driver.Address?.City ?? existing.Address.City;
            existing.Address.State = driver.Address?.State ?? existing.Address.State;
            existing.Address.ZipCode = driver.Address?.ZipCode ?? existing.Address.ZipCode;
        }

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver == null)
            return false;

        _context.Drivers.Remove(driver);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Drivers.AnyAsync(d => d.DriverId == id);
    }

    public async Task<int> GetTotalDriverCountByOfficeAsync(int officeId)
    {
        return await _context.Drivers.CountAsync(d => d.OfficeId == officeId);
    }
}
