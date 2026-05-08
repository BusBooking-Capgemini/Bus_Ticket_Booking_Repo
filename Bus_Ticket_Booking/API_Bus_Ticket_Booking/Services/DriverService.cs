using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.DTOs.Driver;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Services;

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
        return _mapper.Map<IEnumerable<DriverResponseDto>>(drivers);
    }

    public async Task<DriverResponseDto?> GetDriverByIdAsync(int id)
    {
        var driver = await _repo.GetByIdAsync(id);
        return driver == null ? null : _mapper.Map<DriverResponseDto>(driver);
    }

    public async Task<IEnumerable<DriverResponseDto>> GetDriversByOfficeAsync(int officeId)
    {
        var drivers = await _repo.GetByOfficeIdAsync(officeId);
        return _mapper.Map<IEnumerable<DriverResponseDto>>(drivers);
    }

    public async Task<DriverResponseDto?> GetDriverByLicenseAsync(string licenseNumber)
    {
        var driver = await _repo.GetByLicenseNumberAsync(licenseNumber);
        return driver == null ? null : _mapper.Map<DriverResponseDto>(driver);
    }

    public async Task<IEnumerable<DriverResponseDto>> GetDriversByNameAsync(string name)
    {
        var drivers = await _repo.GetByNameAsync(name);
        return _mapper.Map<IEnumerable<DriverResponseDto>>(drivers);
    }

    public async Task<IEnumerable<DriverResponseDto>> GetDriversByCityAsync(string city)
    {
        var drivers = await _repo.GetByCityAsync(city);
        return _mapper.Map<IEnumerable<DriverResponseDto>>(drivers);
    }

    public async Task<DriverResponseDto> CreateDriverAsync(DriverRequestDto dto)
    {
        // Address handled via DTO — create address first
        var address = new Address
        {
            Address1 = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
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
        var driver = _mapper.Map<Driver>(dto);

        // Build address object to pass down for update
        driver.Address = new Address
        {
            Address1 = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
        };

        var updated = await _repo.UpdateAsync(id, driver);
        if (updated == null)
            return null;

        var result = await _repo.GetByIdAsync(updated.DriverId);
        return _mapper.Map<DriverResponseDto>(result!);
    }

    public async Task<bool> DeleteDriverAsync(int id)
    {
        return await _repo.DeleteAsync(id);
    }

    public async Task<int> GetDriverCountByOfficeAsync(int officeId)
    {
        return await _repo.GetTotalDriverCountByOfficeAsync(officeId);
    }
}
