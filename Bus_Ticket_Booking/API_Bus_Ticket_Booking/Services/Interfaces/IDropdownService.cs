using API_Bus_Ticket_Booking.DTOs.Common;

namespace API_Bus_Ticket_Booking.Services.Interfaces
{
    public interface IDropdownService
    {
        Task<List<DropdownDto>>
            GetRoutesDropdownAsync();

        Task<List<DropdownDto>>
            GetBusesDropdownAsync();

        Task<List<DropdownDto>>
            GetDriversDropdownAsync();

        Task<List<DropdownDto>>
            GetAddressesDropdownAsync();

        Task<List<DropdownDto>>
    GetBusesDropdownByOfficeAsync(
        int officeId);

        Task<List<DropdownDto>>
            GetDriversDropdownByOfficeAsync(
                int officeId);
    }
}