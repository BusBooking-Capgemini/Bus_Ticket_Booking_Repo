using API_Bus_Ticket_Booking.DTOs.Office;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [Route("api/offices")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _service;

        public OfficeController(IOfficeService service)
        {
            _service = service;
        }

        // 1. GET all offices
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        // 2. GET office by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            return Ok(result);
        }

        // 3. POST create office
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] OfficeRequestDto dto)
        {
            var result =
                await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.OfficeId },
                result);
        }

        // 4. PUT update office
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] OfficeRequestDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return Ok(new
            {
                Message = "Office updated successfully"
            });
        }

        // 5. DELETE office
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new
            {
                Message = "Office deleted successfully"
            });
        }

        // 6. GET office summary
        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetSummary(int id)
        {
            var result =
                await _service.GetSummaryAsync(id);

            return Ok(result);
        }

        // 7. GET office buses
        [HttpGet("{id}/buses")]
        public async Task<IActionResult> GetBuses(int id)
        {
            var result =
                await _service.GetBusesAsync(id);

            return Ok(result);
        }

        // 8. GET office drivers
        [HttpGet("{id}/drivers")]
        public async Task<IActionResult> GetDrivers(int id)
        {
            var result =
                await _service.GetDriversAsync(id);

            return Ok(result);
        }

        // 9. GET office trips
        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetTrips(int id)
        {
            var result =
                await _service.GetTripsAsync(id);

            return Ok(result);
        }

        // 10. GET office bookings
        [HttpGet("{id}/bookings")]
        public async Task<IActionResult> GetBookings(int id)
        {
            var result =
                await _service.GetBookingsAsync(id);

            return Ok(result);
        }

        // 11. GET office payments
        [HttpGet("{id}/payments")]
        public async Task<IActionResult> GetPayments(int id)
        {
            var result =
                await _service.GetPaymentsAsync(id);

            return Ok(result);
        }
    }
}