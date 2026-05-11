using AutoMapper;
using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.DTOs.Driver;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;

namespace API_Bus_Ticket_Booking.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _repo;
        private readonly IMapper _mapper;
        private readonly BusTicketBookingContext _context;

        public DriverService(IDriverRepository repo, IMapper mapper, BusTicketBookingContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<DriverResponseDto>> GetAllDriversAsync()
        {
            var drivers = await _repo.GetAllAsync();
            if (!drivers.Any())
                throw new NotFoundException("No drivers found.");
            return _mapper.Map<IEnumerable<DriverResponseDto>>(drivers);
        }

        public async Task<DriverResponseDto?> GetDriverByIdAsync(int id)
        {
            var driver = await _repo.GetByIdAsync(id);
            if (driver == null)
                throw new NotFoundException($"Driver with ID {id} not found.");
            return _mapper.Map<DriverResponseDto>(driver);
        }

        public async Task<IEnumerable<DriverResponseDto>> GetDriversByOfficeAsync(int officeId)
        {
            if (officeId <= 0)
                throw new BadRequestException("OfficeId must be a positive number.");
            var drivers = await _repo.GetByOfficeIdAsync(officeId);
            if (!drivers.Any())
                throw new NotFoundException($"No drivers found for office ID {officeId}.");
            return _mapper.Map<IEnumerable<DriverResponseDto>>(drivers);
        }

        public async Task<DriverResponseDto?> GetDriverByLicenseAsync(string licenseNumber)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber))
                throw new BadRequestException("License number cannot be empty.");
            var driver = await _repo.GetByLicenseNumberAsync(licenseNumber);
            if (driver == null)
                throw new NotFoundException($"Driver with license '{licenseNumber}' not found.");
            return _mapper.Map<DriverResponseDto>(driver);
        }

        public async Task<IEnumerable<DriverResponseDto>> GetDriversByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BadRequestException("Search name cannot be empty.");
            var drivers = await _repo.GetByNameAsync(name);
            if (!drivers.Any())
                throw new NotFoundException($"No drivers found with name '{name}'.");
            return _mapper.Map<IEnumerable<DriverResponseDto>>(drivers);
        }

        public async Task<IEnumerable<DriverResponseDto>> GetDriversByCityAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new BadRequestException("City cannot be empty.");
            var drivers = await _repo.GetByCityAsync(city);
            if (!drivers.Any())
                throw new NotFoundException($"No drivers found in city '{city}'.");
            return _mapper.Map<IEnumerable<DriverResponseDto>>(drivers);
        }

        public async Task<DriverResponseDto> CreateDriverAsync(DriverRequestDto dto)
        {
            var existing = await _repo.GetByLicenseNumberAsync(dto.LicenseNumber);
            if (existing != null)
                throw new ConflictException($"Driver with license '{dto.LicenseNumber}' already exists.");

            // Create address via DTO
            var address = new Address
            {
                Address1 = dto.Address,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode
            };
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            var driver = _mapper.Map<Driver>(dto);
            driver.AddressId = address.AddressId;

            var created = await _repo.CreateAsync(driver);
            var result = await _repo.GetByIdAsync(created.DriverId);
            return _mapper.Map<DriverResponseDto>(result!);
        }

        public async Task<DriverResponseDto?> UpdateDriverAsync(int id, DriverRequestDto dto)
        {
            var exists = await _repo.ExistsAsync(id);
            if (!exists)
                throw new NotFoundException($"Driver with ID {id} not found.");

            // Check if license belongs to another driver
            var existing = await _repo.GetByLicenseNumberAsync(dto.LicenseNumber);
            if (existing != null && existing.DriverId != id)
                throw new ConflictException($"License '{dto.LicenseNumber}' is already used by another driver.");

            var driver = _mapper.Map<Driver>(dto);
            driver.Address = new Address
            {
                Address1 = dto.Address,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode
            };

            var updated = await _repo.UpdateAsync(id, driver);
            var result = await _repo.GetByIdAsync(updated!.DriverId);
            return _mapper.Map<DriverResponseDto>(result!);
        }

        public async Task<bool> DeleteDriverAsync(int id)
        {
            var exists = await _repo.ExistsAsync(id);
            if (!exists)
                throw new NotFoundException($"Driver with ID {id} not found.");
            return await _repo.DeleteAsync(id);
        }

        public async Task<int> GetDriverCountByOfficeAsync(int officeId)
        {
            if (officeId <= 0)
                throw new BadRequestException("OfficeId must be a positive number.");
            return await _repo.GetTotalDriverCountByOfficeAsync(officeId);
        }
    }
}