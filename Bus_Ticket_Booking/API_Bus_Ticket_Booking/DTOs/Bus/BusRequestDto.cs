namespace API_Bus_Ticket_Booking.DTOs.Bus;

public class BusRequestDto
{
    public int OfficeId { get; set; }
    public string RegistrationNumber { get; set; } = null!;
    public int Capacity { get; set; }
    public string Type { get; set; } = null!;
}
