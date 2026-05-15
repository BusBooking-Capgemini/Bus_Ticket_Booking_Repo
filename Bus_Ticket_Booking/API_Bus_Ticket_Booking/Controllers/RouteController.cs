using API_Bus_Ticket_Booking.DTOs;
using API_Bus_Ticket_Booking.DTOs.Route;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [ApiController]
    [Route("api/routes")]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        // GET api/routes
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var routes = await _routeService.GetAllRoutesAsync();
            var response = ApiResponse<object>.Ok(
                routes,
                $"{routes.Count} route(s) retrieved successfully."
            );
            return Ok(routes);
        }

        // GET api/routes/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var route = await _routeService.GetRouteByIdAsync(id);
                var response = ApiResponse<object>.Ok(route, "Route retrieved successfully.");
                return Ok(route);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
            }
        }

        // GET api/routes/search?fromCity=Delhi&toCity=Mumbai
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(
            [FromQuery] string fromCity,
            [FromQuery] string toCity
        )
        {
            if (string.IsNullOrWhiteSpace(fromCity) || string.IsNullOrWhiteSpace(toCity))
            {
                return BadRequest(
                    ApiResponse<object>.Fail("fromCity and toCity are required.", 400)
                );
            }

            var routes = await _routeService.SearchRoutesAsync(fromCity, toCity);
            var response = ApiResponse<object>.Ok(
                routes,
                $"{routes.Count} route(s) found matching your search."
            );
            return Ok(routes);
        }

        // GET api/routes/5/trips
        [HttpGet("{id:int}/trips")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTripsForRoute(int id)
        {
            try
            {
                var trips = await _routeService.GetTripsByRouteAsync(id);
                var response = ApiResponse<object>.Ok(
                    trips,
                    $"{trips.Count} trip(s) found for route ID {id}."
                );
                return Ok(trips);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
            }
        }

        // GET api/routes/cities
        [HttpGet("cities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _routeService.GetAllCitiesAsync();
            var response = ApiResponse<object>.Ok(
                cities,
                $"{cities.Count} unique city/cities retrieved."
            );
            return Ok(cities);
        }

        // GET api/routes/from/Delhi
        [HttpGet("from/{city}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByFromCity(string city)
        {
            var routes = await _routeService.GetRoutesByFromCityAsync(city);
            var response = ApiResponse<object>.Ok(
                routes,
                $"{routes.Count} route(s) departing from {city}."
            );
            return Ok(routes);
        }

        // GET api/routes/to/Mumbai
        [HttpGet("to/{city}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByToCity(string city)
        {
            var routes = await _routeService.GetRoutesToCityAsync(city);
            var response = ApiResponse<object>.Ok(
                routes,
                $"{routes.Count} route(s) arriving at {city}."
            );
            return Ok(routes);
        }

        // GET api/routes/duration?max=300
        [HttpGet("duration")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByMaxDuration([FromQuery] int max)
        {
            if (max <= 0)
            {
                return BadRequest(
                    ApiResponse<object>.Fail("max duration must be greater than zero.", 400)
                );
            }
            var routes = await _routeService.GetRoutesByMaxDurationAsync(max);
            var response = ApiResponse<object>.Ok(
                routes,
                $"{routes.Count} route(s) with duration up to {max} minutes."
            );
            return Ok(routes);
        }

        // POST api/routes
        [HttpPost]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> Create([FromBody] CreateRouteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.Fail("Validation failed.", 400));
            }

            try
            {
                var created = await _routeService.CreateRouteAsync(dto);
                var response = ApiResponse<object>.Created(created, "Route created successfully.");
                return CreatedAtAction(nameof(GetById), new { id = created.RouteId }, response);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message, 400));
            }
        }

        // PUT api/routes/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Agency")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRouteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.Fail("Validation failed.", 400));
            }

            try
            {
                var updated = await _routeService.UpdateRouteAsync(id, dto);
                var response = ApiResponse<object>.Ok(updated, "Route updated successfully.");
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
            }
        }

        // DELETE api/routes/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _routeService.DeleteRouteAsync(id);
                var response = ApiResponse<object>.Ok(
                    null,
                    $"Route with ID {id} deleted successfully."
                );
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
            }
        }
    }
}
