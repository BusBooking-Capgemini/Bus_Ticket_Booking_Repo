using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces;

public interface IBusRepository
{
    Task<IEnumerable<Bus>> GetAllAsync();
    Task<Bus?> GetByIdAsync(int id);
    Task<IEnumerable<Bus>> GetByOfficeIdAsync(int officeId);
    Task<IEnumerable<Bus>> GetByTypeAsync(string type);
    Task<Bus?> GetByRegistrationNumberAsync(string regNumber);
    Task<IEnumerable<Bus>> GetByCapacityRangeAsync(int min, int max);
    Task<Bus> CreateAsync(Bus bus);
    Task<Bus?> UpdateAsync(int id, Bus bus);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> GetTotalBusCountByOfficeAsync(int officeId);
}
