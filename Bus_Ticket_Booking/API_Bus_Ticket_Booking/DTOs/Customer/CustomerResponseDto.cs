namespace API_Bus_Ticket_Booking.DTOs.Customer;

public class CustomerResponseDto
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}
