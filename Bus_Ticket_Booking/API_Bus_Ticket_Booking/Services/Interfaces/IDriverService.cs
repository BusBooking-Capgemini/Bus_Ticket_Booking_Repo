public interface IDriverService
{
    Task<IEnumerable<DriverResponseDto>> GetAllDriversAsync();
    Task<DriverResponseDto?> GetDriverByIdAsync(int id);
    Task<IEnumerable<DriverResponseDto>> GetDriversByOfficeAsync(int officeId);
    Task<DriverResponseDto?> GetDriverByLicenseAsync(string licenseNumber);
    Task<IEnumerable<DriverResponseDto>> GetDriversByNameAsync(string name);
    Task<IEnumerable<DriverResponseDto>> GetDriversByCityAsync(string city);
    Task<DriverResponseDto> CreateDriverAsync(DriverRequestDto dto);
    Task<DriverResponseDto?> UpdateDriverAsync(int id, DriverRequestDto dto);
    Task<bool> DeleteDriverAsync(int id);
    Task<int> GetDriverCountByOfficeAsync(int officeId);
}
