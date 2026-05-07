using API_Bus_Ticket_Booking.Models;

public interface IDriverRepository
{
    Task<IEnumerable<Driver>> GetAllAsync();
    Task<Driver?> GetByIdAsync(int id);
    Task<IEnumerable<Driver>> GetByOfficeIdAsync(int officeId);
    Task<Driver?> GetByLicenseNumberAsync(string licenseNumber);
    Task<IEnumerable<Driver>> GetByNameAsync(string name);
    Task<IEnumerable<Driver>> GetByCityAsync(string city);
    Task<Driver> CreateAsync(Driver driver);
    Task<Driver?> UpdateAsync(int id, Driver driver);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> GetTotalDriverCountByOfficeAsync(int officeId);
}