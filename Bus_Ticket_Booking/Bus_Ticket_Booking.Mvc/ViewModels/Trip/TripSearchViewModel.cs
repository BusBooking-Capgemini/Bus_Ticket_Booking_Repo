using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_Booking.Mvc.ViewModels.Trip
{
    public class TripSearchViewModel
    {
        [Required]
        public string FromCity { get; set; }
            = string.Empty;

        [Required]
        public string ToCity { get; set; }
            = string.Empty;

        [Required]
        public DateTime TripDate { get; set; }
            = DateTime.Today;
    }
}