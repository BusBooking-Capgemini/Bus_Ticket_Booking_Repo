namespace API_Bus_Ticket_Booking.DTOs.Review;

public class ReviewCreateDto
{
    public int CustomerId { get; set; }
    public int TripId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
