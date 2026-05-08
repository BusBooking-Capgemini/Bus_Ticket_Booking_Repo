using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces
{
    public interface IAgencyRepository
    {
        Task<IEnumerable<Agency>> GetAllAsync();

        Task<Agency> GetByIdAsync(int id);

        Task UpdateAsync(Agency agency);

        Task DeleteAsync(Agency agency);

        Task<IEnumerable<AgencyOffice>> GetAgencyOfficesAsync(int agencyId);

        Task<IEnumerable<Booking>> GetOfficeBookingsAsync(int agencyId, int officeId);

        Task<IEnumerable<Payment>> GetOfficePaymentsAsync(int agencyId, int officeId);

        Task<IEnumerable<Trip>> GetOfficeTripsAsync(int agencyId, int officeId);

        Task<IEnumerable<Bus>> GetOfficeBusesAsync(int agencyId, int officeId);

        Task<IEnumerable<Driver>> GetOfficeDriversAsync(int agencyId, int officeId);

    }
}
