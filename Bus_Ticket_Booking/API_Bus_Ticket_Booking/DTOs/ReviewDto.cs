namespace API_Bus_Ticket_Booking.DTOs;

public class ReviewCreateDto
{
    public int CustomerId { get; set; }
    public int TripId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}

public class ReviewResponseDto
{
    public int ReviewId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int TripId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime? ReviewDate { get; set; }
}
