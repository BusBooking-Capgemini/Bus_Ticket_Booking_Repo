using API_Bus_Ticket_Booking.DTOs.Trip;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;

namespace API_Bus_Ticket_Booking.Services.Implementations
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IRouteRepository _routeRepository;

        public TripService(ITripRepository tripRepository, IRouteRepository routeRepository)
        {
            _tripRepository = tripRepository;
            _routeRepository = routeRepository;
        }

        public async Task<List<TripResponseDto>> GetAllTripsAsync()
        {
            var trips = await _tripRepository.GetAllAsync();
            var result = new List<TripResponseDto>();
            foreach (var trip in trips)
            {
                result.Add(MapToDto(trip));
            }
            return result;
        }

        public async Task<TripResponseDto> GetTripByIdAsync(int id)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null)
            {
                return null;
            }
            return MapToDto(trip);
        }

        public async Task<List<TripResponseDto>> SearchTripsAsync(TripSearchDto dto)
        {
            var trips = await _tripRepository.SearchAsync(dto.FromCity, dto.ToCity, dto.TripDate);
            var result = new List<TripResponseDto>();
            foreach (var trip in trips)
            {
                result.Add(MapToDto(trip));
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
            var trips = await _tripRepository.GetByRouteIdAsync(routeId);
            var result = new List<TripResponseDto>();
            foreach (var trip in trips)
            {
                result.Add(MapToDto(trip));
            }
            return result;
        }

        public async Task<TripSeatMapDto> GetSeatMapAsync(int tripId)
        {
            bool exists = await _tripRepository.ExistsAsync(tripId);
            if (!exists)
            {
                return null;
            }

            var trip = await _tripRepository.GetByIdAsync(tripId);
            var bookings = await _tripRepository.GetBookingsByTripIdAsync(tripId);

            var seats = new List<SeatStatusDto>();
            foreach (var booking in bookings)
            {
                var seat = new SeatStatusDto();
                seat.SeatNumber = booking.SeatNumber;
                seat.Status = booking.Status;
                seats.Add(seat);
            }

            var seatMap = new TripSeatMapDto();
            seatMap.TripId = tripId;
            seatMap.TotalSeats = trip.Bus != null ? trip.Bus.Capacity : 0;
            seatMap.AvailableSeats = trip.AvailableSeats;
            seatMap.Seats = seats;

            return seatMap;
        }

        public async Task<List<TripResponseDto>> GetTripsByDateAsync(DateTime date)
        {
            var trips = await _tripRepository.GetByDateAsync(date);
            var result = new List<TripResponseDto>();
            foreach (var trip in trips)
            {
                result.Add(MapToDto(trip));
            }
            return result;
        }

        public async Task<List<TripResponseDto>> GetTripsByBusAsync(int busId)
        {
            var trips = await _tripRepository.GetByBusIdAsync(busId);
            var result = new List<TripResponseDto>();
            foreach (var trip in trips)
            {
                result.Add(MapToDto(trip));
            }
            return result;
        }

        public async Task<List<TripResponseDto>> GetTripsByDriverAsync(int driverId)
        {
            var trips = await _tripRepository.GetByDriverIdAsync(driverId);
            var result = new List<TripResponseDto>();
            foreach (var trip in trips)
            {
                result.Add(MapToDto(trip));
            }
            return result;
        }

        public async Task<List<TripResponseDto>> GetUpcomingTripsAsync()
        {
            var trips = await _tripRepository.GetUpcomingAsync();
            var result = new List<TripResponseDto>();
            foreach (var trip in trips)
            {
                result.Add(MapToDto(trip));
            }
            return result;
        }

        public async Task<TripResponseDto> CreateTripAsync(CreateTripDto dto)
        {
            if (dto.Driver1DriverId == dto.Driver2DriverId)
            {
                throw new Exception("Driver 1 and Driver 2 cannot be the same person.");
            }

            if (dto.ArrivalTime <= dto.DepartureTime)
            {
                throw new Exception("Arrival time must be after departure time.");
            }

            bool routeExists = await _routeRepository.ExistsAsync(dto.RouteId);
            if (!routeExists)
            {
                throw new Exception("Route not found.");
            }

            int capacity = await _tripRepository.GetBusCapacityAsync(dto.BusId);
            if (capacity == 0)
            {
                throw new Exception("Bus not found.");
            }

            bool busAvailable = await _tripRepository.IsBusAvailableAsync(
                dto.BusId, dto.TripDate, dto.DepartureTime, dto.ArrivalTime, 0);
            if (!busAvailable)
            {
                throw new Exception("Bus is already assigned to another trip on this schedule.");
            }

            bool driver1Available = await _tripRepository.IsDriverAvailableAsync(
                dto.Driver1DriverId, dto.TripDate, dto.DepartureTime, dto.ArrivalTime, 0);
            if (!driver1Available)
            {
                throw new Exception("Driver 1 is already assigned to another trip on this schedule.");
            }

            bool driver2Available = await _tripRepository.IsDriverAvailableAsync(
                dto.Driver2DriverId, dto.TripDate, dto.DepartureTime, dto.ArrivalTime, 0);
            if (!driver2Available)
            {
                throw new Exception("Driver 2 is already assigned to another trip on this schedule.");
            }

            var trip = new Trip();
            trip.RouteId = dto.RouteId;
            trip.BusId = dto.BusId;
            trip.BoardingAddressId = dto.BoardingAddressId;
            trip.DroppingAddressId = dto.DroppingAddressId;
            trip.DepartureTime = dto.DepartureTime;
            trip.ArrivalTime = dto.ArrivalTime;
            trip.Driver1DriverId = dto.Driver1DriverId;
            trip.Driver2DriverId = dto.Driver2DriverId;
            trip.Fare = dto.Fare;
            trip.TripDate = dto.TripDate;
            trip.AvailableSeats = capacity;

            var created = await _tripRepository.CreateAsync(trip);
            var full = await _tripRepository.GetByIdAsync(created.TripId);
            return MapToDto(full);
        }

        public async Task<TripResponseDto> UpdateTripAsync(int id, UpdateTripDto dto)
        {
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null)
            {
                return null;
            }

            int finalBusId = dto.BusId ?? trip.BusId;
            DateTime finalDeparture = dto.DepartureTime ?? trip.DepartureTime;
            DateTime finalArrival = dto.ArrivalTime ?? trip.ArrivalTime;
            int finalDriver1 = dto.Driver1DriverId ?? trip.Driver1DriverId;
            int finalDriver2 = dto.Driver2DriverId ?? trip.Driver2DriverId;

            if (finalArrival <= finalDeparture)
            {
                throw new Exception("Arrival time must be after departure time.");
            }

            if (finalDriver1 == finalDriver2)
            {
                throw new Exception("Driver 1 and Driver 2 cannot be the same person.");
            }

            bool busAvailable = await _tripRepository.IsBusAvailableAsync(
                finalBusId, trip.TripDate, finalDeparture, finalArrival, id);
            if (!busAvailable)
            {
                throw new Exception("Bus is already assigned to another trip on this schedule.");
            }

            bool driver1Available = await _tripRepository.IsDriverAvailableAsync(
                finalDriver1, trip.TripDate, finalDeparture, finalArrival, id);
            if (!driver1Available)
            {
                throw new Exception("Driver 1 is already assigned to another trip on this schedule.");
            }

            bool driver2Available = await _tripRepository.IsDriverAvailableAsync(
                finalDriver2, trip.TripDate, finalDeparture, finalArrival, id);
            if (!driver2Available)
            {
                throw new Exception("Driver 2 is already assigned to another trip on this schedule.");
            }

            if (dto.BusId != null) trip.BusId = dto.BusId.Value;
            if (dto.BoardingAddressId != null) trip.BoardingAddressId = dto.BoardingAddressId.Value;
            if (dto.DroppingAddressId != null) trip.DroppingAddressId = dto.DroppingAddressId.Value;
            if (dto.DepartureTime != null) trip.DepartureTime = dto.DepartureTime.Value;
            if (dto.ArrivalTime != null) trip.ArrivalTime = dto.ArrivalTime.Value;
            if (dto.Driver1DriverId != null) trip.Driver1DriverId = dto.Driver1DriverId.Value;
            if (dto.Driver2DriverId != null) trip.Driver2DriverId = dto.Driver2DriverId.Value;
            if (dto.Fare != null) trip.Fare = dto.Fare.Value;

            var updated = await _tripRepository.UpdateAsync(trip);
            var full = await _tripRepository.GetByIdAsync(updated.TripId);
            return MapToDto(full);
        }

        public async Task<bool> DeleteTripAsync(int id)
        {
            bool exists = await _tripRepository.ExistsAsync(id);
            if (!exists)
            {
                return false;
            }
            await _tripRepository.DeleteAsync(id);
            return true;
        }

        private TripResponseDto MapToDto(Trip trip)
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
