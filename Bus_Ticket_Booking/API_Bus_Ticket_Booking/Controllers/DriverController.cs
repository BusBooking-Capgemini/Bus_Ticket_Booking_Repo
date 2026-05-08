using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class DriverController : ControllerBase
{
    private readonly IDriverService _service;

    public DriverController(IDriverService service)
    {
        _service = service;
    }

    // 1. GET all drivers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllDriversAsync();
        return Ok(result);
    }

    // 2. GET driver by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetDriverByIdAsync(id);
        if (result == null) return NotFound($"Driver with ID {id} not found.");
        return Ok(result);
    }

    // 3. GET drivers by office
    [HttpGet("office/{officeId}")]
    public async Task<IActionResult> GetByOffice(int officeId)
    {
        var result = await _service.GetDriversByOfficeAsync(officeId);
        return Ok(result);
    }

    // 4. GET driver by license number
    [HttpGet("license/{licenseNumber}")]
    public async Task<IActionResult> GetByLicense(string licenseNumber)
    {
        var result = await _service.GetDriverByLicenseAsync(licenseNumber);
        if (result == null) return NotFound($"Driver with license {licenseNumber} not found.");
        return Ok(result);
    }

    // 5. GET drivers by name search
    [HttpGet("search")]
    public async Task<IActionResult> SearchByName([FromQuery] string name)
    {
        var result = await _service.GetDriversByNameAsync(name);
        return Ok(result);
    }

    // 6. GET drivers by city
    [HttpGet("city/{city}")]
    public async Task<IActionResult> GetByCity(string city)
    {
        var result = await _service.GetDriversByCityAsync(city);
        return Ok(result);
    }

    // 7. GET driver count by office
    [HttpGet("office/{officeId}/count")]
    public async Task<IActionResult> GetDriverCount(int officeId)
    {
        var count = await _service.GetDriverCountByOfficeAsync(officeId);
        return Ok(new { OfficeId = officeId, TotalDrivers = count });
    }

    // 8. POST create driver
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DriverRequestDto dto)
    {
        var result = await _service.CreateDriverAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.DriverId }, result);
    }

    // 9. PUT update driver
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DriverRequestDto dto)
    {
        var result = await _service.UpdateDriverAsync(id, dto);
        if (result == null) return NotFound($"Driver with ID {id} not found.");
        return Ok(result);
    }

    // 10. DELETE driver
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteDriverAsync(id);
        if (!deleted) return NotFound($"Driver with ID {id} not found.");
        return Ok(new { Message = $"Driver {id} deleted successfully." });
    }
}