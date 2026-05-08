namespace API_Bus_Ticket_Booking.DTOs.Trip;

public class TripResponseDto
{
    public int TripId { get; set; }
    public string FromCity { get; set; } = null!;
    public string ToCity { get; set; } = null!;
    public DateTime TripDate { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AvailableSeats { get; set; }
    public decimal Fare { get; set; }
    public string BusType { get; set; } = null!;
    public int? BreakPoints { get; set; }
    public int? Duration { get; set; }
}
