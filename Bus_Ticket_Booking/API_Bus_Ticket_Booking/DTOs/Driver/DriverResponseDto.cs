namespace API_Bus_Ticket_Booking.DTOs.Driver;

public class DriverResponseDto
{
    public int DriverId { get; set; }
    public string LicenseNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int OfficeId { get; set; }
    public string OfficeName { get; set; } = null!;

    // Flattened address
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
}
