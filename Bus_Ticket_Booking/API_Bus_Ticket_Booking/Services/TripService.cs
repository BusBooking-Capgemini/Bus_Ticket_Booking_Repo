using API_Bus_Ticket_Booking.DTOs.Trip;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IMapper _mapper;
        private readonly IRouteRepository _routeRepository;

        public TripService(
            ITripRepository tripRepository,
            IRouteRepository routeRepository,
            IMapper mapper
        )
        {
            _tripRepository = tripRepository;
            _routeRepository = routeRepository;
            _mapper = mapper;
        }

        private async Task<TripResponseDto> BuildTripDtoAsync(Trip trip)
        {
            var dto = _mapper.Map<TripResponseDto>(trip);

            var bookings = await _tripRepository.GetBookingsByTripIdAsync(trip.TripId);

            int bookedSeats = bookings.Count(b => b.Status == "Booked");

            int totalSeats = trip.Bus != null ? trip.Bus.Capacity : 0;

            dto.AvailableSeats = totalSeats - bookedSeats;

            if (dto.AvailableSeats < 0)
            {
                dto.AvailableSeats = 0;
            }

            return dto;
        }

        public async Task<List<TripResponseDto>> GetAllTripsAsync()
        {
            var trips = await _tripRepository.GetAllAsync();

            var result = new List<TripResponseDto>();

            foreach (var trip in trips)
            {
                result.Add(await BuildTripDtoAsync(trip));
            }

            return result;
        }

        public async Task<TripResponseDto> GetTripByIdAsync(int id)
        {
            var trip = await _tripRepository.GetByIdAsync(id);

            if (trip == null)
            {
                throw new NotFoundException($"Trip with ID {id} was not found.");
            }

            return await BuildTripDtoAsync(trip);
        }

        public async Task<List<TripResponseDto>> SearchTripsAsync(TripSearchDto dto)
        {
            var trips = await _tripRepository.SearchAsync(dto.FromCity, dto.ToCity, dto.TripDate);

            var result = new List<TripResponseDto>();

            foreach (var trip in trips)
            {
                result.Add(await BuildTripDtoAsync(trip));
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

            var trips = await _tripRepository.GetByRouteIdAsync(routeId);

            var result = new List<TripResponseDto>();

            foreach (var trip in trips)
            {
                result.Add(await BuildTripDtoAsync(trip));
            }

            return result;
        }

        public async Task<TripSeatMapDto> GetSeatMapAsync(int tripId)
        {
            bool exists = await _tripRepository.ExistsAsync(tripId);

            if (!exists)
            {
                throw new NotFoundException($"Trip with ID {tripId} was not found.");
            }

            var trip = await _tripRepository.GetByIdAsync(tripId);

            var bookings = await _tripRepository.GetBookingsByTripIdAsync(tripId);

            int totalSeats = trip.Bus.Capacity;

            var availableSeatNumbers = bookings
                .Where(b => b.Status == "Available")
                .Select(b => b.SeatNumber)
                .ToList();

            // Console.WriteLine(bookedSeatNumbers);
            Console.WriteLine("start");
            // foreach (var item in bookedSeatNumbers)
            // {
            //     Console.WriteLine(item);
            // }
            Console.WriteLine("end");

            var seats = new List<SeatStatusDto>();

            for (int i = 1; i <= totalSeats; i++)
            {
                seats.Add(
                    new SeatStatusDto
                    {
                        SeatNumber = i,
                        Status = availableSeatNumbers.Contains(i) ? "Available" : "Booked",
                    }
                );
            }

            return new TripSeatMapDto
            {
                TripId = tripId,
                TotalSeats = totalSeats,
                AvailableSeats = availableSeatNumbers.Count,
                Seats = seats,
            };
        }

        public async Task<List<TripResponseDto>> GetTripsByDateAsync(DateTime date)
        {
            var trips = await _tripRepository.GetByDateAsync(date);

            var result = new List<TripResponseDto>();

            foreach (var trip in trips)
            {
                result.Add(await BuildTripDtoAsync(trip));
            }

            return result;
        }

        public async Task<List<TripResponseDto>> GetTripsByBusAsync(int busId)
        {
            var trips = await _tripRepository.GetByBusIdAsync(busId);

            var result = new List<TripResponseDto>();

            foreach (var trip in trips)
            {
                result.Add(await BuildTripDtoAsync(trip));
            }

            return result;
        }

        public async Task<List<TripResponseDto>> GetTripsByDriverAsync(int driverId)
        {
            var trips = await _tripRepository.GetByDriverIdAsync(driverId);

            var result = new List<TripResponseDto>();

            foreach (var trip in trips)
            {
                result.Add(await BuildTripDtoAsync(trip));
            }

            return result;
        }

        public async Task<List<TripResponseDto>> GetUpcomingTripsAsync()
        {
            var trips = await _tripRepository.GetUpcomingAsync();

            var result = new List<TripResponseDto>();

            foreach (var trip in trips)
            {
                result.Add(await BuildTripDtoAsync(trip));
            }

            return result;
        }

        public async Task<TripResponseDto> CreateTripAsync(CreateTripDto dto)
        {
            int capacity = await _tripRepository.GetBusCapacityAsync(dto.BusId);

            var trip = _mapper.Map<Trip>(dto);

            trip.AvailableSeats = capacity;

            var createdTrip = await _tripRepository.CreateAsync(trip);

            var seatEntries = new List<Booking>();

            for (int seat = 1; seat <= capacity; seat++)
            {
                seatEntries.Add(
                    new Booking
                    {
                        TripId = createdTrip.TripId,

                        SeatNumber = seat,

                        Status = "Available",
                    }
                );
            }

            await _tripRepository.CreateSeatEntriesAsync(seatEntries);

            var fullTrip = await _tripRepository.GetByIdAsync(createdTrip.TripId);

            return await BuildTripDtoAsync(fullTrip!);
        }

        public async Task<TripResponseDto> UpdateTripAsync(int id, UpdateTripDto dto)
        {
            var trip = await _tripRepository.GetByIdAsync(id);

            if (trip == null)
            {
                throw new NotFoundException($"Trip with ID {id} was not found.");
            }

            if (dto.BusId != null)
                trip.BusId = dto.BusId.Value;

            if (dto.BoardingAddressId != null)
                trip.BoardingAddressId = dto.BoardingAddressId.Value;

            if (dto.DroppingAddressId != null)
                trip.DroppingAddressId = dto.DroppingAddressId.Value;

            if (dto.DepartureTime != null)
                trip.DepartureTime = dto.DepartureTime.Value;

            if (dto.ArrivalTime != null)
                trip.ArrivalTime = dto.ArrivalTime.Value;

            if (dto.Driver1DriverId != null)
                trip.Driver1DriverId = dto.Driver1DriverId.Value;

            if (dto.Driver2DriverId != null)
                trip.Driver2DriverId = dto.Driver2DriverId.Value;

            if (dto.Fare != null)
                trip.Fare = dto.Fare.Value;

            if (dto.TripDate != null)
                trip.TripDate = dto.TripDate.Value;

            var updated = await _tripRepository.UpdateAsync(trip);

            var full = await _tripRepository.GetByIdAsync(updated.TripId);

            return await BuildTripDtoAsync(full);
        }

        public async Task<bool> DeleteTripAsync(int id)
        {
            bool exists = await _tripRepository.ExistsAsync(id);

            if (!exists)
            {
                throw new NotFoundException($"Trip with ID {id} was not found.");
            }

            await _tripRepository.DeleteAsync(id);

            return true;
        }

        public async Task<List<TripResponseDto>> GetTripsByOfficeAsync(int officeId)
        {
            var trips = await _tripRepository.GetByOfficeIdAsync(officeId);

            var result = new List<TripResponseDto>();

            foreach (var trip in trips)
            {
                result.Add(await BuildTripDtoAsync(trip));
            }

            return result;
        }

        public async Task<List<TripResponseDto>> GetTripsByAgencyAsync(int agencyId)
        {
            var trips = await _tripRepository.GetByAgencyIdAsync(agencyId);

            var result = new List<TripResponseDto>();

            foreach (var trip in trips)
            {
                result.Add(await BuildTripDtoAsync(trip));
            }

            return result;
        }
    }
}
