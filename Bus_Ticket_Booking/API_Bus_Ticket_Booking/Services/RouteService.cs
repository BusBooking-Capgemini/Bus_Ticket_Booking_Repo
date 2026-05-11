using API_Bus_Ticket_Booking.DTOs.Route;
using API_Bus_Ticket_Booking.DTOs.Trip;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;

namespace API_Bus_Ticket_Booking.Services.Implementations
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;

        public RouteService(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<List<RouteresponseDto>> GetAllRoutesAsync()
        {
            var routes = await _routeRepository.GetAllAsync();
            var result = new List<RouteresponseDto>();
            foreach (var route in routes)
            {
                result.Add(MapRouteToDto(route));
            }
            return result;
        }

        public async Task<RouteresponseDto> GetRouteByIdAsync(int id)
        {
            var route = await _routeRepository.GetByIdAsync(id);
            if (route == null)
            {
                return null;
            }
            return MapRouteToDto(route);
        }

        public async Task<List<RouteresponseDto>> SearchRoutesAsync(string fromCity, string toCity)
        {
            var routes = await _routeRepository.SearchAsync(fromCity, toCity);
            var result = new List<RouteresponseDto>();
            foreach (var route in routes)
            {
                result.Add(MapRouteToDto(route));
            }
            return result;
        }

        public async Task<List<TripResponseDto>> GetTripsByRouteAsync(int routeId)
        {
            bool exists = await _routeRepository.ExistsAsync(routeId);
            if (!exists)
            {
                return null;
            }
            var trips = await _routeRepository.GetTripsByRouteIdAsync(routeId);
            var result = new List<TripResponseDto>();
            foreach (var trip in trips)
            {
                result.Add(MapTripToDto(trip));
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
                result.Add(MapRouteToDto(route));
            }
            return result;
        }

        public async Task<List<RouteresponseDto>> GetRoutesToCityAsync(string city)
        {
            var routes = await _routeRepository.GetByCityAsync(city);
            var result = new List<RouteresponseDto>();
            foreach (var route in routes)
            {
                result.Add(MapRouteToDto(route));
            }
            return result;
        }

        public async Task<List<RouteresponseDto>> GetRoutesByMaxDurationAsync(int maxMinutes)
        {
            var routes = await _routeRepository.GetByMaxDurationAsync(maxMinutes);
            var result = new List<RouteresponseDto>();
            foreach (var route in routes)
            {
                result.Add(MapRouteToDto(route));
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
            return MapRouteToDto(created);
        }

        public async Task<RouteresponseDto> UpdateRouteAsync(int id, UpdateRouteDto dto)
        {
            var route = await _routeRepository.GetByIdAsync(id);
            if (route == null)
            {
                return null;
            }

            if (dto.FromCity != null)
            {
                route.FromCity = dto.FromCity;
            }
            if (dto.ToCity != null)
            {
                route.ToCity = dto.ToCity;
            }
            if (dto.BreakPoints != null)
            {
                route.BreakPoints = dto.BreakPoints;
            }
            if (dto.Duration != null)
            {
                route.Duration = dto.Duration;
            }

            var updated = await _routeRepository.UpdateAsync(route);
            return MapRouteToDto(updated);
        }

        public async Task<bool> DeleteRouteAsync(int id)
        {
            bool exists = await _routeRepository.ExistsAsync(id);
            if (!exists)
            {
                return false;
            }
            await _routeRepository.DeleteAsync(id);
            return true;
        }

        private RouteresponseDto MapRouteToDto(API_Bus_Ticket_Booking.Models.Route route)
        {
            var dto = new RouteresponseDto();
            dto.RouteId = route.RouteId;
            dto.FromCity = route.FromCity;
            dto.ToCity = route.ToCity;
            dto.BreakPoints = route.BreakPoints;
            dto.Duration = route.Duration;
            return dto;
        }

        private TripResponseDto MapTripToDto(API_Bus_Ticket_Booking.Models.Trip trip)
        {
            var dto = new TripResponseDto();
            dto.TripId = trip.TripId;
            dto.RouteId = trip.RouteId;
            dto.FromCity = trip.Route != null ? trip.Route.FromCity : "";
            dto.ToCity = trip.Route != null ? trip.Route.ToCity : "";
            dto.BusId = trip.BusId;
            dto.BusType = trip.Bus != null ? trip.Bus.Type : "";
            dto.BoardingCity = "";
            dto.DroppingCity = "";
            dto.DepartureTime = trip.DepartureTime;
            dto.ArrivalTime = trip.ArrivalTime;
            dto.Driver1Name = trip.Driver1Driver != null ? trip.Driver1Driver.Name : "";
            dto.Driver2Name = trip.Driver2Driver != null ? trip.Driver2Driver.Name : "";
            dto.AvailableSeats = trip.AvailableSeats;
            dto.Fare = trip.Fare;
            dto.TripDate = trip.TripDate;
            return dto;
        }
    }
}
