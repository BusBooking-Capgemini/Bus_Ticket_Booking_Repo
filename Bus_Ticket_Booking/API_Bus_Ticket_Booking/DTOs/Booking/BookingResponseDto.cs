namespace API_Bus_Ticket_Booking.DTOs.Booking;

public class BookingResponseForCustomerDto
{
    public int BookingId { get; set; }
    public int TripId { get; set; }
    public string FromCity { get; set; } = null!;
    public string ToCity { get; set; } = null!;
    public DateTime TripDate { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int SeatNumber { get; set; }
    public string Status { get; set; } = null!;
    public decimal Fare { get; set; }
    public decimal? AmountPaid { get; set; }
    public string? PaymentStatus { get; set; }
}
