using Microsoft.AspNetCore.Http;
using API_Bus_Ticket_Booking.DTOs.Agency;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [Route("api/agencies")]
    [ApiController]
    public class AgencyController : ControllerBase
    {
        private readonly IAgencyService _service;

        public AgencyController(IAgencyService service)
        {
            _service = service;
        }

        // 1. GET all agencies
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        // 2. GET agency by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            return Ok(result);
        }

        // 3. PUT update agency
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] AgencyRequestDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return Ok(new
            {
                Message = "Agency updated successfully"
            });
        }

        // 4. DELETE agency
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new
            {
                Message = "Agency deleted successfully"
            });
        }

        // 5. GET all offices by agency
        [HttpGet("{id}/offices")]
        public async Task<IActionResult> GetAgencyOffices(int id)
        {
            var result =
                await _service.GetAgencyOfficesAsync(id);

            return Ok(result);
        }

        // 6. GET agency summary
        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetAgencySummary(int id)
        {
            var result =
                await _service.GetAgencySummaryAsync(id);

            return Ok(result);
        }

        // 7. GET office bookings by agency
        [HttpGet("{agencyId}/offices/{officeId}/bookings")]
        public async Task<IActionResult> GetOfficeBookings(
            int agencyId,
            int officeId)
        {
            var result =
                await _service.GetOfficeBookingsAsync(
                    agencyId,
                    officeId);

            return Ok(result);
        }

        // 8. GET office payments by agency
        [HttpGet("{agencyId}/offices/{officeId}/payments")]
        public async Task<IActionResult> GetOfficePayments(
            int agencyId,
            int officeId)
        {
            var result =
                await _service.GetOfficePaymentsAsync(
                    agencyId,
                    officeId);

            return Ok(result);
        }

        // 9. GET office trips by agency
        [HttpGet("{agencyId}/offices/{officeId}/trips")]
        public async Task<IActionResult> GetOfficeTrips(
            int agencyId,
            int officeId)
        {
            var result =
                await _service.GetOfficeTripsAsync(
                    agencyId,
                    officeId);

            return Ok(result);
        }

        // 10. GET office buses by agency
        [HttpGet("{agencyId}/offices/{officeId}/buses")]
        public async Task<IActionResult> GetOfficeBuses(
            int agencyId,
            int officeId)
        {
            var result =
                await _service.GetOfficeBusesAsync(
                    agencyId,
                    officeId);

            return Ok(result);
        }

        // 11. GET office drivers by agency
        [HttpGet("{agencyId}/offices/{officeId}/drivers")]
        public async Task<IActionResult> GetOfficeDrivers(
            int agencyId,
            int officeId)
        {
            var result =
                await _service.GetOfficeDriversAsync(
                    agencyId,
                    officeId);

            return Ok(result);
        }
    }
}