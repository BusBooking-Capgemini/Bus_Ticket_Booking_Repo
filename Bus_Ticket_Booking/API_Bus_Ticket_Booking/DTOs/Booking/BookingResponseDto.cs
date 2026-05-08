namespace API_Bus_Ticket_Booking.DTOs.Booking;

public class BookingResponseDto
{
    public int BookingId { get; set; }
    public int TripId { get; set; }
    public string FromCity { get; set; }
    public string ToCity { get; set; }
    public DateTime TripDate { get; set; }
    public int SeatNumber { get; set; }
    public string Status { get; set; }
    public decimal? AmountPaid { get; set; }
    public string? PaymentStatus { get; set; }
}
