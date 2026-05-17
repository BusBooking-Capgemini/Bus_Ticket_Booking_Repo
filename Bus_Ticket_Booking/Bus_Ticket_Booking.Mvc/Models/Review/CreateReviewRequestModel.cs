namespace Bus_Ticket_Booking.Mvc.Models.Review
{
    public class CreateReviewRequestModel
    {
        public int? TripId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }
}
