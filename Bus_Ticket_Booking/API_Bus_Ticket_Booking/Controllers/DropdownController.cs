using API_Bus_Ticket_Booking.DTOs;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [ApiController]
    [Route("api/dropdowns")]
    [Authorize(Roles = "Office,Agency")]
    public class DropdownController : ControllerBase
    {
        private readonly IDropdownService
            _dropdownService;

        public DropdownController(
            IDropdownService dropdownService)
        {
            _dropdownService =
                dropdownService;
        }


        [HttpGet("routes")]
        public async Task<IActionResult>
            GetRoutes()
        {
            var result =
                await _dropdownService
                    .GetRoutesDropdownAsync();

            return Ok(
                ApiResponse<object>.Ok(
                    result,
                    "Routes dropdown fetched successfully."));
        }

        [HttpGet("buses/{officeId:int}")]
        public async Task<IActionResult>
    GetBuses(
        int officeId)
        {
            var result =
                await _dropdownService
                    .GetBusesDropdownByOfficeAsync(
                        officeId);

            return Ok(
                ApiResponse<object>.Ok(
                    result,
                    "Buses dropdown fetched successfully."));
        }


        [HttpGet("drivers/{officeId:int}")]
        public async Task<IActionResult>
    GetDrivers(
        int officeId)
        {
            var result =
                await _dropdownService
                    .GetDriversDropdownByOfficeAsync(
                        officeId);

            return Ok(
                ApiResponse<object>.Ok(
                    result,
                    "Drivers dropdown fetched successfully."));
        }


        [HttpGet("addresses")]
        public async Task<IActionResult>
            GetAddresses()
        {
            var result =
                await _dropdownService
                    .GetAddressesDropdownAsync();

            return Ok(
                ApiResponse<object>.Ok(
                    result,
                    "Addresses dropdown fetched successfully."));
        }
    }
}