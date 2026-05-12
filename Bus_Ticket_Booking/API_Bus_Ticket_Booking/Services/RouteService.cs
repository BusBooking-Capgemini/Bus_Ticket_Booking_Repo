using API_Bus_Ticket_Booking.DTOs.Route;
using API_Bus_Ticket_Booking.DTOs.Trip;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IMapper _mapper;

        public RouteService(IRouteRepository routeRepository, IMapper mapper)
        {
            _routeRepository = routeRepository;
            _mapper = mapper;
        }

        public async Task<List<RouteresponseDto>> GetAllRoutesAsync()
        {
            var routes = await _routeRepository.GetAllAsync();
            var result = new List<RouteresponseDto>();
            foreach (var route in routes)
            {
                result.Add(_mapper.Map<RouteresponseDto>(route));
            }
            return result;
        }

        public async Task<RouteresponseDto> GetRouteByIdAsync(int id)
        {
            var route = await _routeRepository.GetByIdAsync(id);
            if (route == null)
            {
                throw new NotFoundException($"Route with ID {id} was not found.");
            }
            return _mapper.Map<RouteresponseDto>(route);
        }

        public async Task<List<RouteresponseDto>> SearchRoutesAsync(string fromCity, string toCity)
        {
            var routes = await _routeRepository.SearchAsync(fromCity, toCity);
            var result = new List<RouteresponseDto>();
            foreach (var route in routes)
            {
                result.Add(_mapper.Map<RouteresponseDto>(route));
            }
            return result;
        }

        public async Task<List<TripResponseDto>> GetTripsByRouteAsync(int routeId)
        {
            bool exists = await _routeRepository.ExistsAsync(routeId);
            if (!exists)
            {
                throw new NotFoundException($"Route with ID {routeId} was not found.");
            }
            var trips = await _routeRepository.GetTripsByRouteIdAsync(routeId);
            var result = new List<TripResponseDto>();
            foreach (var trip in trips)
            {
                result.Add(_mapper.Map<TripResponseDto>(trip));
            }
            return result;
        }

        public async Task<List<string>> GetAllCitiesAsync()
        {
            var routes = await _routeRepository.GetAllAsync();
            var cities = new List<string>();
            foreach (var route in routes)
            {
                if (!cities.Contains(route.FromCity))
                {
                    cities.Add(route.FromCity);
                }
                if (!cities.Contains(route.ToCity))
                {
                    cities.Add(route.ToCity);
                }
            }
            cities.Sort();
            return cities;
        }

        public async Task<List<RouteresponseDto>> GetRoutesByFromCityAsync(string city)
        {
            var routes = await _routeRepository.GetByFromCityAsync(city);
            var result = new List<RouteresponseDto>();
            foreach (var route in routes)
            {
                result.Add(_mapper.Map<RouteresponseDto>(route));
            }
            return result;
        }

        public async Task<List<RouteresponseDto>> GetRoutesToCityAsync(string city)
        {
            var routes = await _routeRepository.GetByCityAsync(city);
            var result = new List<RouteresponseDto>();
            foreach (var route in routes)
            {
                result.Add(_mapper.Map<RouteresponseDto>(route));
            }
            return result;
        }

        public async Task<List<RouteresponseDto>> GetRoutesByMaxDurationAsync(int maxMinutes)
        {
            var routes = await _routeRepository.GetByMaxDurationAsync(maxMinutes);
            var result = new List<RouteresponseDto>();
            foreach (var route in routes)
            {
                result.Add(_mapper.Map<RouteresponseDto>(route));
            }
            return result;
        }

        public async Task<RouteresponseDto> CreateRouteAsync(CreateRouteDto dto)
        {
            var route = new API_Bus_Ticket_Booking.Models.Route();
            route.FromCity = dto.FromCity;
            route.ToCity = dto.ToCity;
            route.BreakPoints = dto.BreakPoints;
            route.Duration = dto.Duration;

            var created = await _routeRepository.CreateAsync(route);
            return _mapper.Map<RouteresponseDto>(created);
        }

        public async Task<RouteresponseDto> UpdateRouteAsync(int id, UpdateRouteDto dto)
        {
            var route = await _routeRepository.GetByIdAsync(id);
            if (route == null)
            {
                throw new NotFoundException($"Route with ID {id} was not found.");
            }

            if (dto.FromCity != null) route.FromCity = dto.FromCity;
            if (dto.ToCity != null) route.ToCity = dto.ToCity;
            if (dto.BreakPoints != null) route.BreakPoints = dto.BreakPoints;
            if (dto.Duration != null) route.Duration = dto.Duration;

            var updated = await _routeRepository.UpdateAsync(route);
            return _mapper.Map<RouteresponseDto>(updated);
        }

        public async Task<bool> DeleteRouteAsync(int id)
        {
            bool exists = await _routeRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new NotFoundException($"Route with ID {id} was not found.");
            }
            await _routeRepository.DeleteAsync(id);
            return true;
        }
    }
}
