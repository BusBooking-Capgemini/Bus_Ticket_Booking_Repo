using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces
{
    public interface IOfficeRepository
    {
        Task<IEnumerable<AgencyOffice>> GetAllAsync();

        Task<AgencyOffice> GetByIdAsync(int id);

        Task AddAsync(AgencyOffice office); // by Agency

        Task UpdateAsync(AgencyOffice office);

        Task DeleteAsync(AgencyOffice office);

        Task<IEnumerable<Bus>> GetBusesAsync(int officeId);

        Task<IEnumerable<Driver>> GetDriversAsync(int officeId);

        Task<IEnumerable<Trip>> GetTripsAsync(int officeId);

        Task<IEnumerable<Booking>> GetBookingsAsync(int officeId);

        Task<IEnumerable<Payment>> GetPaymentsAsync(int officeId);
    }
}
