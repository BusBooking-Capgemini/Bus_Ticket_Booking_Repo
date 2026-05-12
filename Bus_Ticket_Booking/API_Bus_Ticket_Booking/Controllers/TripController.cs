using API_Bus_Ticket_Booking.DTOs;
using API_Bus_Ticket_Booking.DTOs.Trip;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        // GET api/trips
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var trips = await _tripService.GetAllTripsAsync();
            var response = ApiResponse<object>.Ok(trips, $"{trips.Count} trip(s) retrieved successfully.");
            return Ok(response);
        }

        // GET api/trips/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var trip = await _tripService.GetTripByIdAsync(id);
                var response = ApiResponse<object>.Ok(trip, "Trip retrieved successfully.");
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
            }
        }

        // GET api/trips/search?fromCity=Delhi&toCity=Mumbai&tripDate=2026-06-01
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] TripSearchDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.Fail("Validation failed. Please check fromCity, toCity and tripDate.", 400));
            }

            var trips = await _tripService.SearchTripsAsync(dto);
            var response = ApiResponse<object>.Ok(trips, $"{trips.Count} trip(s) found from {dto.FromCity} to {dto.ToCity} on {dto.TripDate:yyyy-MM-dd}.");
            return Ok(response);
        }

        // GET api/trips/5/seats
        [HttpGet("{id:int}/seats")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSeatMap(int id)
        {
            try
            {
                var seatMap = await _tripService.GetSeatMapAsync(id);
                var response = ApiResponse<object>.Ok(seatMap, $"Seat map retrieved. {seatMap.AvailableSeats} seat(s) available out of {seatMap.TotalSeats}.");
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
            }
        }

        // GET api/trips/route/3
        [HttpGet("route/{routeId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByRoute(int routeId)
        {
            try
            {
                var trips = await _tripService.GetTripsByRouteAsync(routeId);
                var response = ApiResponse<object>.Ok(trips, $"{trips.Count} trip(s) found for route ID {routeId}.");
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
            }
        }

        // GET api/trips/date/2026-06-01
        [HttpGet("date/{date:datetime}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var trips = await _tripService.GetTripsByDateAsync(date);
            var response = ApiResponse<object>.Ok(trips, $"{trips.Count} trip(s) scheduled on {date:yyyy-MM-dd}.");
            return Ok(response);
        }

        // GET api/trips/bus/2
        [HttpGet("bus/{busId:int}")]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> GetByBus(int busId)
        {
            var trips = await _tripService.GetTripsByBusAsync(busId);
            var response = ApiResponse<object>.Ok(trips, $"{trips.Count} trip(s) assigned to bus ID {busId}.");
            return Ok(response);
        }

        // GET api/trips/driver/5
        [HttpGet("driver/{driverId:int}")]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> GetByDriver(int driverId)
        {
            var trips = await _tripService.GetTripsByDriverAsync(driverId);
            var response = ApiResponse<object>.Ok(trips, $"{trips.Count} trip(s) assigned to driver ID {driverId}.");
            return Ok(response);
        }

        // GET api/trips/upcoming
        [HttpGet("upcoming")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUpcoming()
        {
            var trips = await _tripService.GetUpcomingTripsAsync();
            var response = ApiResponse<object>.Ok(trips, $"{trips.Count} upcoming trip(s) found.");
            return Ok(response);
        }

        // POST api/trips
        [HttpPost]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> Create([FromBody] CreateTripDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.Fail("Validation failed. Please check all required fields.", 400));
            }

            try
            {
                var created = await _tripService.CreateTripAsync(dto);
                var response = ApiResponse<object>.Created(created, "Trip created successfully.");
                return CreatedAtAction(nameof(GetById), new { id = created.TripId }, response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message, 400));
            }
            catch (ConflictException ex)
            {
                return Conflict(ApiResponse<object>.Fail(ex.Message, 409));
            }
        }

        // PUT api/trips/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTripDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.Fail("Validation failed. Please check the provided fields.", 400));
            }

            try
            {
                var updated = await _tripService.UpdateTripAsync(id, dto);
                var response = ApiResponse<object>.Ok(updated, "Trip updated successfully.");
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message, 400));
            }
            catch (ConflictException ex)
            {
                return Conflict(ApiResponse<object>.Fail(ex.Message, 409));
            }
        }

        // DELETE api/trips/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tripService.DeleteTripAsync(id);
                var response = ApiResponse<object>.Ok(null, $"Trip with ID {id} deleted successfully.");
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
            }
        }
    }
}
