using AutoMapper;
using API_Bus_Ticket_Booking.DTOs.Bus;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;

namespace API_Bus_Ticket_Booking.Services
{
    public class BusService : IBusService
    {
        private readonly IBusRepository _repo;
        private readonly IMapper _mapper;

        public BusService(IBusRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BusResponseDto>> GetAllBusesAsync()
        {
            var buses = await _repo.GetAllAsync();
            if (!buses.Any())
                throw new NotFoundException("No buses found.");
            return _mapper.Map<IEnumerable<BusResponseDto>>(buses);
        }

        public async Task<BusResponseDto?> GetBusByIdAsync(int id)
        {
            var bus = await _repo.GetByIdAsync(id);
            if (bus == null)
                throw new NotFoundException($"Bus with ID {id} not found.");
            return _mapper.Map<BusResponseDto>(bus);
        }

        public async Task<IEnumerable<BusResponseDto>> GetBusesByOfficeAsync(int officeId)
        {
            if (officeId <= 0)
                throw new BadRequestException("OfficeId must be a positive number.");
            var buses = await _repo.GetByOfficeIdAsync(officeId);
            if (!buses.Any())
                throw new NotFoundException($"No buses found for office ID {officeId}.");
            return _mapper.Map<IEnumerable<BusResponseDto>>(buses);
        }

        public async Task<IEnumerable<BusResponseDto>> GetBusesByTypeAsync(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new BadRequestException("Bus type cannot be empty.");
            var buses = await _repo.GetByTypeAsync(type);
            if (!buses.Any())
                throw new NotFoundException($"No buses found with type '{type}'.");
            return _mapper.Map<IEnumerable<BusResponseDto>>(buses);
        }

        public async Task<BusResponseDto?> GetBusByRegistrationAsync(string regNumber)
        {
            if (string.IsNullOrWhiteSpace(regNumber))
                throw new BadRequestException("Registration number cannot be empty.");
            var bus = await _repo.GetByRegistrationNumberAsync(regNumber);
            if (bus == null)
                throw new NotFoundException($"Bus with registration '{regNumber}' not found.");
            return _mapper.Map<BusResponseDto>(bus);
        }

        public async Task<IEnumerable<BusResponseDto>> GetBusesByCapacityRangeAsync(int min, int max)
        {
            if (min < 0 || max < min)
                throw new BadRequestException("Invalid range. min must be >= 0 and max must be >= min.");
            var buses = await _repo.GetByCapacityRangeAsync(min, max);
            if (!buses.Any())
                throw new NotFoundException($"No buses found with capacity between {min} and {max}.");
            return _mapper.Map<IEnumerable<BusResponseDto>>(buses);
        }

        public async Task<BusResponseDto> CreateBusAsync(BusRequestDto dto)
        {
            var existing = await _repo.GetByRegistrationNumberAsync(dto.RegistrationNumber);
            if (existing != null)
                throw new ConflictException($"Bus with registration '{dto.RegistrationNumber}' already exists.");

            var bus = _mapper.Map<Bus>(dto);
            var created = await _repo.CreateAsync(bus);
            var result = await _repo.GetByIdAsync(created.BusId);
            return _mapper.Map<BusResponseDto>(result!);
        }

        public async Task<BusResponseDto?> UpdateBusAsync(int id, BusRequestDto dto)
        {
            var exists = await _repo.ExistsAsync(id);
            if (!exists)
                throw new NotFoundException($"Bus with ID {id} not found.");

            // Check if registration belongs to another bus
            var existing = await _repo.GetByRegistrationNumberAsync(dto.RegistrationNumber);
            if (existing != null && existing.BusId != id)
                throw new ConflictException($"Registration '{dto.RegistrationNumber}' is already used by another bus.");

            var bus = _mapper.Map<Bus>(dto);
            var updated = await _repo.UpdateAsync(id, bus);
            var result = await _repo.GetByIdAsync(updated!.BusId);
            return _mapper.Map<BusResponseDto>(result!);
        }

        public async Task<bool> DeleteBusAsync(int id)
        {
            var exists = await _repo.ExistsAsync(id);
            if (!exists)
                throw new NotFoundException($"Bus with ID {id} not found.");
            return await _repo.DeleteAsync(id);
        }

        public async Task<int> GetBusCountByOfficeAsync(int officeId)
        {
            if (officeId <= 0)
                throw new BadRequestException("OfficeId must be a positive number.");
            return await _repo.GetTotalBusCountByOfficeAsync(officeId);
        }
    }
}