using API_Bus_Ticket_Booking.DTOs.Common;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;

namespace API_Bus_Ticket_Booking.Services
{
    public class DropdownService
        : IDropdownService
    {
        private readonly IDropdownRepository
            _repo;

        public DropdownService(
            IDropdownRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DropdownDto>>
            GetRoutesDropdownAsync()
        {
            return await _repo
                .GetRoutesDropdownAsync();
        }

        public async Task<List<DropdownDto>>
            GetBusesDropdownAsync()
        {
            return await _repo
                .GetBusesDropdownAsync();
        }

        public async Task<List<DropdownDto>>
            GetDriversDropdownAsync()
        {
            return await _repo
                .GetDriversDropdownAsync();
        }

        public async Task<List<DropdownDto>>
            GetAddressesDropdownAsync()
        {
            return await _repo
                .GetAddressesDropdownAsync();
        }

        public async Task<List<DropdownDto>>
    GetBusesDropdownByOfficeAsync(
        int officeId)
        {
            return await _repo
                .GetBusesDropdownByOfficeAsync(
                    officeId);
        }

        public async Task<List<DropdownDto>>
    GetDriversDropdownByOfficeAsync(
        int officeId)
        {
            return await _repo
                .GetDriversDropdownByOfficeAsync(
                    officeId);
        }
    }
}