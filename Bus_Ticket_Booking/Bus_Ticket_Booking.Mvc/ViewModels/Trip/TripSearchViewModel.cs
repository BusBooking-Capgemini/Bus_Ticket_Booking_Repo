using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_Booking.Mvc.ViewModels.Trip
{
    public class TripSearchViewModel
    {
        public string? FromCity { get; set; }

        public string? ToCity { get; set; }

        public DateTime? TripDate { get; set; }
    }
}