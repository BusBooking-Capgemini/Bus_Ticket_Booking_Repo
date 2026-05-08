namespace API_Bus_Ticket_Booking.DTOs.Trip;

public class TripSearchDto
{
    public string? FromCity { get; set; }
    public string? ToCity { get; set; }
    public DateTime? TripDate { get; set; }
    public int? MinSeats { get; set; }
    public decimal? MaxFare { get; set; }
}
