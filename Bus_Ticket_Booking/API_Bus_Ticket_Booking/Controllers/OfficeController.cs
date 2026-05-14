using API_Bus_Ticket_Booking.DTOs.Office;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [Route("api/offices")]
    [ApiController]
    [Authorize(Roles = "Office,Agency")]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _service;

        public OfficeController(IOfficeService service)
        {
            _service = service;
        }

        // 1. GET all offices
        [HttpGet]
        [Authorize(Roles = "Agency")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(new
            {
                success = true,
                message = "Offices retrieved successfully",
                count = result.Count(),
                data = result
            });
        }

        // 2. GET office by id
        [HttpGet("{id}")]
        [Authorize(Roles = "Office,Agency")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            return Ok(new
            {
                success = true,
                message = "Office retrieved successfully",
                data = result
            });
        }

        // 3. POST create office
        [HttpPost]
        [Authorize(Roles = "Agency")]
        public async Task<IActionResult> Create([FromBody] OfficeRequestDto dto)
        {
            var result = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.OfficeId },
                new
                {
                    success = true,
                    message = "Office created successfully",
                    data = result
                });
        }

        // 4. PUT update office
        [HttpPut("{id}")]
        [Authorize(Roles = "Office,Agency")]
        public async Task<IActionResult> Update(int id, [FromBody] OfficeRequestDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return Ok(new
            {
                success = true,
                message = "Office updated successfully"
            });
        }

        // 5. DELETE office
        [HttpDelete("{id}")]
        [Authorize(Roles = "Agency")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new
            {
                success = true,
                message = "Office deleted successfully"
            });
        }

        // 6. GET office summary
        [HttpGet("{id}/summary")]
        [Authorize(Roles = "Office,Agency")]
        public async Task<IActionResult> GetSummary(int id)
        {
            var result = await _service.GetSummaryAsync(id);

            return Ok(new
            {
                success = true,
                message = "Office summary retrieved successfully",
                data = result
            });
        }

        // 7. GET office buses
        [HttpGet("{id}/buses")]
        [Authorize(Roles = "Office,Agency")]
        public async Task<IActionResult> GetBuses(int id)
        {
            var result = await _service.GetBusesAsync(id);

            return Ok(new
            {
                success = true,
                message = "Office buses retrieved successfully",
                count = result.Count(),
                data = result
            });
        }

        // 8. GET office drivers
        [HttpGet("{id}/drivers")]
        [Authorize(Roles = "Office,Agency")]
        public async Task<IActionResult> GetDrivers(int id)
        {
            var result = await _service.GetDriversAsync(id);

            return Ok(new
            {
                success = true,
                message = "Office drivers retrieved successfully",
                count = result.Count(),
                data = result
            });
        }

        // 9. GET office trips
        [HttpGet("{id}/trips")]
        [Authorize(Roles = "Office,Agency")]
        public async Task<IActionResult> GetTrips(int id)
        {
            var result = await _service.GetTripsAsync(id);

            return Ok(new
            {
                success = true,
                message = "Office trips retrieved successfully",
                count = result.Count(),
                data = result
            });
        }

        // 10. GET office bookings
        [HttpGet("{id}/bookings")]
        [Authorize(Roles = "Office,Agency")]
        public async Task<IActionResult> GetBookings(int id)
        {
            var result = await _service.GetBookingsAsync(id);

            return Ok(new
            {
                success = true,
                message = "Office bookings retrieved successfully",
                count = result.Count(),
                data = result
            });
        }

        // 11. GET office payments
        [HttpGet("{id}/payments")]  
        [Authorize(Roles = "Office,Agency")]
        public async Task<IActionResult> GetPayments(int id)
        {
            var result = await _service.GetPaymentsAsync(id);

            return Ok(new
            {
                success = true,
                message = "Office payments retrieved successfully",
                count = result.Count(),
                data = result
            });
        }
    }
}