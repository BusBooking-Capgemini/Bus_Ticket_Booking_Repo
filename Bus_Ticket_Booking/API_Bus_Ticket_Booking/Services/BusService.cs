using API_Bus_Ticket_Booking.Models;
using AutoMapper;


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
        return _mapper.Map<IEnumerable<BusResponseDto>>(buses);
    }

    public async Task<BusResponseDto?> GetBusByIdAsync(int id)
    {
        var bus = await _repo.GetByIdAsync(id);
        return bus == null ? null : _mapper.Map<BusResponseDto>(bus);
    }

    public async Task<IEnumerable<BusResponseDto>> GetBusesByOfficeAsync(int officeId)
    {
        var buses = await _repo.GetByOfficeIdAsync(officeId);
        return _mapper.Map<IEnumerable<BusResponseDto>>(buses);
    }

    public async Task<IEnumerable<BusResponseDto>> GetBusesByTypeAsync(string type)
    {
        var buses = await _repo.GetByTypeAsync(type);
        return _mapper.Map<IEnumerable<BusResponseDto>>(buses);
    }

    public async Task<BusResponseDto?> GetBusByRegistrationAsync(string regNumber)
    {
        var bus = await _repo.GetByRegistrationNumberAsync(regNumber);
        return bus == null ? null : _mapper.Map<BusResponseDto>(bus);
    }

    public async Task<IEnumerable<BusResponseDto>> GetBusesByCapacityRangeAsync(int min, int max)
    {
        var buses = await _repo.GetByCapacityRangeAsync(min, max);
        return _mapper.Map<IEnumerable<BusResponseDto>>(buses);
    }

    public async Task<BusResponseDto> CreateBusAsync(BusRequestDto dto)
    {
        var bus = _mapper.Map<Bus>(dto);
        var created = await _repo.CreateAsync(bus);
        var result = await _repo.GetByIdAsync(created.BusId);
        return _mapper.Map<BusResponseDto>(result!);
    }
    public async Task<BusResponseDto?> UpdateBusAsync(int id, BusRequestDto dto)
    {
        var bus = _mapper.Map<Bus>(dto);
        var updated = await _repo.UpdateAsync(id, bus);
        if (updated == null) return null;
        var result = await _repo.GetByIdAsync(updated.BusId);
        return _mapper.Map<BusResponseDto>(result!);
    }
    public async Task<bool> DeleteBusAsync(int id)
    {
        return await _repo.DeleteAsync(id);
    }
    
    public async Task<int> GetBusCountByOfficeAsync(int officeId)
    {
        return await _repo.GetTotalBusCountByOfficeAsync(officeId);
    }
}
