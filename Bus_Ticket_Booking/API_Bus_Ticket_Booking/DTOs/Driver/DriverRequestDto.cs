namespace API_Bus_Ticket_Booking.DTOs.Driver;

public class DriverRequestDto
{
    public string LicenseNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int OfficeId { get; set; }

    // Address fields via DTO (no separate address controller)
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
}
