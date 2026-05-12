using API_Bus_Ticket_Booking.DTOs.Office;

namespace API_Bus_Ticket_Booking.Services.Interfaces
{
    public interface IOfficeService
    {
        Task<IEnumerable<OfficeResponseDto>> GetAllAsync();

        Task<OfficeResponseDto> GetByIdAsync(int id);

        Task<OfficeResponseDto> CreateAsync(OfficeRequestDto dto);

        Task UpdateAsync(int id, OfficeRequestDto dto); // Dto update

        Task DeleteAsync(int id);

        Task<object> GetSummaryAsync(int officeId); // can change output if demands

        Task<IEnumerable<object>> GetBusesAsync(int officeId);

        Task<IEnumerable<object>> GetDriversAsync(int officeId);

        Task<IEnumerable<object>> GetTripsAsync(int officeId);

        Task<IEnumerable<object>> GetBookingsAsync(int officeId);

        Task<IEnumerable<object>> GetPaymentsAsync(int officeId);
    }
}
