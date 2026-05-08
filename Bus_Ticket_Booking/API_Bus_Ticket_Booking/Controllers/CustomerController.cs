using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.DTOs;
using API_Bus_Ticket_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context)
    {
        _context = context;
    }

    // --------------------------------------------
    // CUSTOMER CRUD
    // --------------------------------------------

    // POST /api/customers -> Register a new customer
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto dto)
    {
        var address = new Address
        {
            Address1 = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
        };
        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();

        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            AddressId = address.AddressId,
        };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCustomer),
            new { customerId = customer.CustomerId },
            new
            {
                customer.CustomerId,
                customer.Name,
                customer.Email,
            }
        );
    }

    // GET /api/customers/{customerId} -> Get customer profile
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomer(int customerId)
    {
        var c = await _context
            .Customers.Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

        if (c == null)
            return NotFound();

        return Ok(
            new CustomerResponseDto
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                City = c.Address?.City,
                State = c.Address?.State,
            }
        );
    }

    //  PUT /api/customers/{cusomterId} -> Update custoemr profile
    [HttpPut("{customerId}")]
    public async Task<IActionResult> UpdateCustomer(
        int customerId,
        [FromBody] CustomerUpdateDto dto
    )
    {
        // find customers in db by id
        var customer = await _context.Customers.FindAsync(customerId);

        // customer was not found
        if (customer == null)
            return NotFound();

        // will not update if given field is null, empty or "string". "string" is what comes from swagger if you don't enter anything
        if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != "string")
        {
            customer.Name = dto.Name;
        }

        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != "string")
        {
            customer.Email = dto.Email;
        }

        if (!string.IsNullOrWhiteSpace(dto.Phone) && dto.Phone != "string")
        {
            customer.Phone = dto.Phone;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/customers/{customerId} -> Delete customer account
    [HttpDelete("{customerId}")] // DELETE /api/customers/{customerId}
    public async Task<IActionResult> DeleteCustomer(int customerId)
    {
        // find customer in db
        var customer = await _context.Customers.FindAsync(customerId);

        // if customer not found
        if (customer == null)
            return NotFound();

        // remove customer
        _context.Customers.Remove(customer);

        // save changes
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
