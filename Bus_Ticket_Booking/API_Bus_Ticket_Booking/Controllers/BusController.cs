using API_Bus_Ticket_Booking.DTOs.Bus;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusController : ControllerBase
{
    private readonly IBusService _service;

    public BusController(IBusService service)
    {
        _service = service;
    }

    // 1. GET all buses
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllBusesAsync();
        return Ok(result);
    }

    // 2. GET bus by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetBusByIdAsync(id);
        return Ok(result);
    }

    // 3. GET buses by office
    [HttpGet("office/{officeId}")]
    public async Task<IActionResult> GetByOffice(int officeId)
    {
        var result = await _service.GetBusesByOfficeAsync(officeId);
        return Ok(result);
    }

    // 4. GET buses by type
    [HttpGet("type/{type}")]
    public async Task<IActionResult> GetByType(string type)
    {
        var result = await _service.GetBusesByTypeAsync(type);
        return Ok(result);
    }

    // 5. GET bus by registration number
    [HttpGet("registration/{regNumber}")]
    public async Task<IActionResult> GetByRegistration(string regNumber)
    {
        var result = await _service.GetBusByRegistrationAsync(regNumber);
        return Ok(result);
    }

    // 6. GET buses by capacity range
    [HttpGet("capacity")]
    public async Task<IActionResult> GetByCapacityRange([FromQuery] int min, [FromQuery] int max)
    {
        var result = await _service.GetBusesByCapacityRangeAsync(min, max);
        return Ok(result);
    }

    // 7. GET bus count by office
    [HttpGet("office/{officeId}/count")]
    public async Task<IActionResult> GetBusCount(int officeId)
    {
        var count = await _service.GetBusCountByOfficeAsync(officeId);
        return Ok(new { OfficeId = officeId, TotalBuses = count });
    }

    // 8. POST create bus
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BusRequestDto dto)
    {
        var result = await _service.CreateBusAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.BusId }, result);
    }

    // 9. PUT update bus
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BusRequestDto dto)
    {
        var result = await _service.UpdateBusAsync(id, dto);
        return Ok(result);
    }

    // 10. DELETE bus
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteBusAsync(id);
        return Ok(new { Message = $"Bus {id} deleted successfully." });
    }
}
