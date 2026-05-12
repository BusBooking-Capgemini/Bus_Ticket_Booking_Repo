namespace API_Bus_Ticket_Booking.DTOs.Review;

public class ReviewRequestDto
{
    /// <summary>
    /// This DTO will be used whenever the data is coming from client.
    /// </summary>
    // why is everything nullable?
    // Because it mixes both create and update.
    // Update doesn't have 'TripId'.
    // Create doesn't have 'ReviewId'.
    // Why doesn't create have 'ReviewId', becauase it is not auto generated in the table, we have to create in the controller.

    public int? ReviewId { get; set; }
    public int? TripId { get; set; }
    public int? Rating { get; set; }
    public string? Comment { get; set; }
}
