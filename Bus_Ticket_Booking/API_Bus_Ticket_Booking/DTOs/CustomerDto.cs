namespace API_Bus_Ticket_Booking.DTOs;

public class CustomerCreateDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}

public class CustomerResponseDto
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}

public class CustomerUpdateDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
