using System.ComponentModel.DataAnnotations;

namespace API_Bus_Ticket_Booking.DTOs.Route
{
    public class CreateRouteDto
    {
        [Required]
        public string FromCity { get; set;}

        [Required]

        public string ToCity { get; set;}

        public int? BreakPoints {get; set;}

        public int? Duration {get; set;}
    }

    public class UpdateRouteDto
    {
        public string FromCity {get; set;}
        public string ToCity {get; set;}
        public int? BreakPoints {get; set;}
        public int? Duration {get; set;}
    }

    public class RouteresponseDto
    {
        
        public int RouteId { get; set;}
        public string FromCity {get; set;}

        public string ToCity {get; set;}

        public int? BreakPoints {get; set;}

        public int? Duration {get; set;}
    }
}