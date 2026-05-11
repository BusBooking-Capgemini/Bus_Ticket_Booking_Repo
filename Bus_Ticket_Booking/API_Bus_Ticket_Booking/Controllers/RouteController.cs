using API_Bus_Ticket_Booking.DTOs.Route;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var routes = await _routeService.GetAllRoutesAsync();
            return Ok(routes);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var route = await _routeService.GetRouteByIdAsync(id);
            if (route == null)
            {
                return NotFound(new { message = "Route not found." });
            }
            return Ok(route);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] string fromCity, [FromQuery] string toCity)
        {
            if (string.IsNullOrWhiteSpace(fromCity) || string.IsNullOrWhiteSpace(toCity))
            {
                return BadRequest(new { message = "fromCity and toCity are required." });
            }
            var routes = await _routeService.SearchRoutesAsync(fromCity, toCity);
            return Ok(routes);
        }

        [HttpGet("{id}/trips")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTripsForRoute(int id)
        {
            var trips = await _routeService.GetTripsByRouteAsync(id);
            if (trips == null)
            {
                return NotFound(new { message = "Route not found." });
            }
            return Ok(trips);
        }

        [HttpGet("cities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _routeService.GetAllCitiesAsync();
            return Ok(cities);
        }

        [HttpGet("from/{city}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByFromCity(string city)
        {
            var routes = await _routeService.GetRoutesByFromCityAsync(city);
            return Ok(routes);
        }

        [HttpGet("to/{city}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByToCity(string city)
        {
            var routes = await _routeService.GetRoutesToCityAsync(city);
            return Ok(routes);
        }

        [HttpGet("duration")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByMaxDuration([FromQuery] int max)
        {
            if (max <= 0)
            {
                return BadRequest(new { message = "max must be greater than zero." });
            }
            var routes = await _routeService.GetRoutesByMaxDurationAsync(max);
            return Ok(routes);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> Create([FromBody] CreateRouteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var created = await _routeService.CreateRouteAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.RouteId }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRouteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _routeService.UpdateRouteAsync(id, dto);
            if (updated == null)
            {
                return NotFound(new { message = "Route not found." });
            }
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _routeService.DeleteRouteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Route not found." });
            }
            return NoContent();
        }
    }
}
