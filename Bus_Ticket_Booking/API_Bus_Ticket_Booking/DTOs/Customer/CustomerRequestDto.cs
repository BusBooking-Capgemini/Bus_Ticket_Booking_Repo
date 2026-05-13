namespace API_Bus_Ticket_Booking.DTOs.Customer;

public class CustomerRequestDto
{
    // For CREATE:
    // All fields are required through validation.

    // For UPDATE:
    // Only provided fields will be updated.

    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
}
