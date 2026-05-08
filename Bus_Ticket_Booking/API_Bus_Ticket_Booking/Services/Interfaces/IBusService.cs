public interface IBusService
{
    Task<IEnumerable<BusResponseDto>> GetAllBusesAsync();
    Task<BusResponseDto?> GetBusByIdAsync(int id);
    Task<IEnumerable<BusResponseDto>> GetBusesByOfficeAsync(int officeId);
    Task<IEnumerable<BusResponseDto>> GetBusesByTypeAsync(string type);
    Task<BusResponseDto?> GetBusByRegistrationAsync(string regNumber);
    Task<IEnumerable<BusResponseDto>> GetBusesByCapacityRangeAsync(int min, int max);
    Task<BusResponseDto> CreateBusAsync(BusRequestDto dto);
    Task<BusResponseDto?> UpdateBusAsync(int id, BusRequestDto dto);
    Task<bool> DeleteBusAsync(int id);
    Task<int> GetBusCountByOfficeAsync(int officeId);
}
