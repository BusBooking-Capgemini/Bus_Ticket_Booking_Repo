using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.DTOs.Booking;
using API_Bus_Ticket_Booking.DTOs.Customer;
using API_Bus_Ticket_Booking.DTOs.Review;
using API_Bus_Ticket_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly BusTicketBookingContext _context;

    public CustomerController(BusTicketBookingContext context)
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

    // --------------------------------------------
    // REVIEWS
    // --------------------------------------------

    // POST /api/customers/{customerId}/reviews -> Submit a review
    [HttpPost("{customerId}/reviews")]
    public async Task<IActionResult> AddReview(int customerId, [FromBody] ReviewCreateDto dto)
    {
        if (customerId != dto.CustomerId)
            return BadRequest("Customer ID mismatch.");

        // Validate customer exists
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
            return NotFound("Customer not found.");

        // Validate trip exists
        var trip = await _context.Trips.FindAsync(dto.TripId);
        if (trip == null)
            return NotFound("Trip not found.");

        // Check customer actually booked this trip
        var didBook = await _context.Payments.AnyAsync(p =>
            p.CustomerId == customerId
            && p.Booking.TripId == dto.TripId
            && p.PaymentStatus == "Success"
        );

        if (!didBook)
            return BadRequest("You can only review trips you have completed.");

        // Validate rating range
        if (dto.Rating < 1 || dto.Rating > 5)
            return BadRequest("Rating must be between 1 and 5.");

        // reviews.review_id is NOT IDENTITY, so we need to generate it manually
        int nextId = await _context.Reviews.AnyAsync()
            ? await _context.Reviews.MaxAsync(r => r.ReviewId) + 1
            : 1;

        // Create review
        var review = new Review
        {
            ReviewId = nextId,
            CustomerId = customerId,
            TripId = dto.TripId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            ReviewDate = DateTime.Now,
        };

        // Add review
        _context.Reviews.Add(review);

        // Update database
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetReview),
            new { customerId, reviewId = review.ReviewId },
            new
            {
                review.ReviewId,
                review.Rating,
                review.ReviewDate,
            }
        );
    }

    // GET /api/customers/{customerId}/reviews -> All reviews made by a customer
    [HttpGet("{customerId}/reviews")]
    public async Task<IActionResult> GetCustomerReviews(int customerId)
    {
        // check if customer exists
        var exists = await _context.Customers.AnyAsync(c => c.CustomerId == customerId);

        // if customer doesn't exist
        if (!exists)
            return NotFound("Customer not found.");

        // get reviews
        var reviews = await _context
            .Reviews.Where(r => r.CustomerId == customerId)
            .Include(r => r.Customer)
            .Include(r => r.Trip)
                .ThenInclude(t => t.Route)
            .Select(r => new ReviewResponseDto
            {
                ReviewId = r.ReviewId,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer.Name,
                TripId = r.TripId,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate,
            })
            .ToListAsync();

        return Ok(reviews);
    }

    // GET /api/customers/{customerId}/reviews/{reviewId}  → Single review
    [HttpGet("{customerId}/reviews/{reviewId}")]
    public async Task<IActionResult> GetReview(int customerId, int reviewId)
    {
        // check if review exists
        var review = await _context
            .Reviews.Include(r => r.Customer)
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.CustomerId == customerId);

        // if review doesn't exist
        if (review == null)
            return NotFound();

        return Ok(
            new ReviewResponseDto
            {
                ReviewId = review.ReviewId,
                CustomerId = review.CustomerId,
                CustomerName = review.Customer.Name,
                TripId = review.TripId,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate,
            }
        );
    }

    // DELETE /api/customers/{customerId}/reviews/{reviewId}  → Delete a review
    [HttpDelete("{customerId}/reviews/{reviewId}")]
    public async Task<IActionResult> DeleteReview(int customerId, int reviewId)
    {
        // check if review exists
        var review = await _context.Reviews.FirstOrDefaultAsync(r =>
            r.ReviewId == reviewId && r.CustomerId == customerId
        );

        // if review doesn't exist
        if (review == null)
            return NotFound();

        // remove review
        _context.Reviews.Remove(review);

        // update database
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
