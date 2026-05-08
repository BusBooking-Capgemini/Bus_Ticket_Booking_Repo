namespace API_Bus_Ticket_Booking.DTOs.Review;

public class ReviewResponseDto
{
    public int ReviewId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public int TripId { get; set; }
    public string FromCity { get; set; } = null!;
    public string ToCity { get; set; } = null!;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime? ReviewDate { get; set; }
}
