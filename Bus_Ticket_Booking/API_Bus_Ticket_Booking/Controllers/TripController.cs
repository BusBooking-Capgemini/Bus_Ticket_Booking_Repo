using API_Bus_Ticket_Booking.DTOs.Trip;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var trips = await _tripService.GetAllTripsAsync();
            return Ok(trips);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null)
            {
                return NotFound(new { message = "Trip not found." });
            }
            return Ok(trip);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] TripSearchDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var trips = await _tripService.SearchTripsAsync(dto);
            return Ok(trips);
        }

        [HttpGet("{id}/seats")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSeatMap(int id)
        {
            var seatMap = await _tripService.GetSeatMapAsync(id);
            if (seatMap == null)
            {
                return NotFound(new { message = "Trip not found." });
            }
            return Ok(seatMap);
        }

        [HttpGet("route/{routeId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByRoute(int routeId)
        {
            var trips = await _tripService.GetTripsByRouteAsync(routeId);
            if (trips == null)
            {
                return NotFound(new { message = "Route not found." });
            }
            return Ok(trips);
        }

        [HttpGet("date/{date}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var trips = await _tripService.GetTripsByDateAsync(date);
            return Ok(trips);
        }

        [HttpGet("bus/{busId}")]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> GetByBus(int busId)
        {
            var trips = await _tripService.GetTripsByBusAsync(busId);
            return Ok(trips);
        }

        [HttpGet("driver/{driverId}")]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> GetByDriver(int driverId)
        {
            var trips = await _tripService.GetTripsByDriverAsync(driverId);
            return Ok(trips);
        }

        [HttpGet("upcoming")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUpcoming()
        {
            var trips = await _tripService.GetUpcomingTripsAsync();
            return Ok(trips);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> Create([FromBody] CreateTripDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var created = await _tripService.CreateTripAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.TripId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTripDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updated = await _tripService.UpdateTripAsync(id, dto);
                if (updated == null)
                {
                    return NotFound(new { message = "Trip not found." });
                }
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _tripService.DeleteTripAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Trip not found." });
            }
            return NoContent();
        }
    }
}
