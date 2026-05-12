using API_Bus_Ticket_Booking.DTOs.Bus;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [ApiController]
    [Route("api/buses")]
    public class BusController : ControllerBase
    {
        private readonly IBusService _service;

        public BusController(IBusService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllBusesAsync();
            return Ok(new
            {
                success = true,
                message = "Buses retrieved successfully.",
                count = result.Count(),
                data = result
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetBusByIdAsync(id);
            return Ok(new
            {
                success = true,
                message = "Bus retrieved successfully.",
                data = result
            });
        }

        [HttpGet("office/{officeId:int}")]
        public async Task<IActionResult> GetByOffice(int officeId)
        {
            var result = await _service.GetBusesByOfficeAsync(officeId);
            return Ok(new
            {
                success = true,
                message = $"Buses for office ID {officeId} retrieved successfully.",
                count = result.Count(),
                data = result
            });
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var result = await _service.GetBusesByTypeAsync(type);
            return Ok(new
            {
                success = true,
                message = $"Buses of type '{type}' retrieved successfully.",
                count = result.Count(),
                data = result
            });
        }

        [HttpGet("registration/{regNumber}")]
        public async Task<IActionResult> GetByRegistration(string regNumber)
        {
            var result = await _service.GetBusByRegistrationAsync(regNumber);
            return Ok(new
            {
                success = true,
                message = "Bus retrieved successfully.",
                data = result
            });
        }

        [HttpGet("capacity")]
        public async Task<IActionResult> GetByCapacityRange([FromQuery] int min, [FromQuery] int max)
        {
            var result = await _service.GetBusesByCapacityRangeAsync(min, max);
            return Ok(new
            {
                success = true,
                message = $"Buses with capacity between {min} and {max} retrieved successfully.",
                count = result.Count(),
                data = result
            });
        }

        [HttpGet("office/{officeId:int}/count")]
        public async Task<IActionResult> GetBusCount(int officeId)
        {
            var count = await _service.GetBusCountByOfficeAsync(officeId);
            return Ok(new
            {
                success = true,
                message = $"Bus count for office ID {officeId} retrieved successfully.",
                officeId,
                totalBuses = count
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusRequestDto dto)
        {
            var result = await _service.CreateBusAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.BusId }, new
            {
                success = true,
                message = "Bus created successfully.",
                data = result
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusRequestDto dto)
        {
            var result = await _service.UpdateBusAsync(id, dto);
            return Ok(new
            {
                success = true,
                message = $"Bus with ID {id} updated successfully.",
                data = result
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteBusAsync(id);
            return Ok(new
            {
                success = true,
                message = $"Bus with ID {id} deleted successfully."
            });
        }
    }
}