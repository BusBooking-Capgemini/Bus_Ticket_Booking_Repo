using API_Bus_Ticket_Booking.DTOs.Agency;

namespace API_Bus_Ticket_Booking.Services.Interfaces
{
    public interface IAgencyService
    {
        Task<IEnumerable<AgencyResponseDto>> GetAllAsync();

        Task<AgencyResponseDto> GetByIdAsync(int id);

        Task UpdateAsync(int id, AgencyRequestDto dto); // Dto update

        Task DeleteAsync(int id);

        Task<IEnumerable<object>> GetAgencyOfficesAsync(int agencyId);

        Task<object> GetAgencySummaryAsync(int agencyId); // can change output if demands

        Task<IEnumerable<object>> GetOfficeBookingsAsync(int agencyId, int officeId);

        Task<IEnumerable<object>> GetOfficePaymentsAsync(int agencyId, int officeId);

        Task<IEnumerable<object>> GetOfficeTripsAsync(int agencyId, int officeId);

        Task<IEnumerable<object>> GetOfficeBusesAsync(int agencyId, int officeId);

        Task<IEnumerable<object>> GetOfficeDriversAsync(int agencyId, int officeId);
    }
}
