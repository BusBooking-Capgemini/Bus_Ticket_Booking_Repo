using API_Bus_Ticket_Booking.DTOs.Driver;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [ApiController]
    [Route("api/drivers")]
    [Authorize(Roles = "Office,Agency")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _service;

        public DriverController(IDriverService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllDriversAsync();
            return Ok(new
            {
                success = true,
                message = "Drivers retrieved successfully.",
                count = result.Count(),
                data = result
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetDriverByIdAsync(id);
            return Ok(new
            {
                success = true,
                message = "Driver retrieved successfully.",
                data = result
            });
        }

        [HttpGet("office/{officeId:int}")]
        public async Task<IActionResult> GetByOffice(int officeId)
        {
            var result = await _service.GetDriversByOfficeAsync(officeId);
            return Ok(new
            {
                success = true,
                message = $"Drivers for office ID {officeId} retrieved successfully.",
                count = result.Count(),
                data = result
            });
        }

        [HttpGet("license/{licenseNumber}")]
        public async Task<IActionResult> GetByLicense(string licenseNumber)
        {
            var result = await _service.GetDriverByLicenseAsync(licenseNumber);
            return Ok(new
            {
                success = true,
                message = "Driver retrieved successfully.",
                data = result
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            var result = await _service.GetDriversByNameAsync(name);
            return Ok(new
            {
                success = true,
                message = $"Drivers matching '{name}' retrieved successfully.",
                count = result.Count(),
                data = result
            });
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetByCity(string city)
        {
            var result = await _service.GetDriversByCityAsync(city);
            return Ok(new
            {
                success = true,
                message = $"Drivers in city '{city}' retrieved successfully.",
                count = result.Count(),
                data = result
            });
        }

        [HttpGet("office/{officeId:int}/count")]
        public async Task<IActionResult> GetDriverCount(int officeId)
        {
            var count = await _service.GetDriverCountByOfficeAsync(officeId);
            return Ok(new
            {
                success = true,
                message = $"Driver count for office ID {officeId} retrieved successfully.",
                officeId,
                totalDrivers = count
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DriverRequestDto dto)
        {
            var result = await _service.CreateDriverAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.DriverId }, new
            {
                success = true,
                message = "Driver created successfully.",
                data = result
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] DriverRequestDto dto)
        {
            var result = await _service.UpdateDriverAsync(id, dto);
            return Ok(new
            {
                success = true,
                message = $"Driver with ID {id} updated successfully.",
                data = result
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteDriverAsync(id);
            return Ok(new
            {
                success = true,
                message = $"Driver with ID {id} deleted successfully."
            });
        }
    }
}