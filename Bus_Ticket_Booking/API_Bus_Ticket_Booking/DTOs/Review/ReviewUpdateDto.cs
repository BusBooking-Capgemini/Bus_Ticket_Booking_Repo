namespace API_Bus_Ticket_Booking.DTOs.Review;

public class ReviewUpdateDto
{
    public int ReviewId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
