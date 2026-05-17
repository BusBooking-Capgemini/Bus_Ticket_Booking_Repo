using System.ComponentModel.DataAnnotations;

namespace API_Bus_Ticket_Booking.DTOs.Trip
{
    public class CreateTripDto
    {
        [Required]
        public int RouteId { get; set; }

        [Required]
        public int BusId { get; set; }

        [Required]
        public int BoardingAddressId { get; set; }

        [Required]
        public int DroppingAddressId { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public int Driver1DriverId { get; set; }

        [Required]
        public int Driver2DriverId { get; set; }

        [Required]
        public decimal Fare { get; set; }

        [Required]
        public DateTime TripDate { get; set; }
    }

    public class UpdateTripDto
    {
        public int? BusId { get; set; }

        public int? BoardingAddressId { get; set; }

        public int? DroppingAddressId { get; set; }

        public DateTime? DepartureTime { get; set; }

        public DateTime? ArrivalTime { get; set; }

        public int? Driver1DriverId { get; set; }

        public int? Driver2DriverId { get; set; }

        public decimal? Fare { get; set; }

        public DateTime? TripDate { get; set; }
    }

    public class TripResponseDto
    {
        public int TripId { get; set; }

        public int RouteId { get; set; }

        public string FromCity { get; set; }
            = string.Empty;

        public string ToCity { get; set; }
            = string.Empty;

        public int BusId { get; set; }

        public string BusType { get; set; }
            = string.Empty;

        public int BoardingAddressId { get; set; }

        public int DroppingAddressId { get; set; }

        public string BoardingCity { get; set; }
            = string.Empty;

        public string DroppingCity { get; set; }
            = string.Empty;

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public int Driver1DriverId { get; set; }

        public int Driver2DriverId { get; set; }

        public string Driver1Name { get; set; }
            = string.Empty;

        public string Driver2Name { get; set; }
            = string.Empty;

        public int AvailableSeats { get; set; }

        public decimal Fare { get; set; }

        public DateTime TripDate { get; set; }
    }

    public class TripSearchDto
    {
        [Required]
        public string FromCity { get; set; }

        [Required]
        public string ToCity { get; set; }

        [Required]
        public DateTime TripDate { get; set; }
    }

    public class SeatStatusDto
    {
        public int SeatNumber { get; set; }
        public string Status { get; set; }
    }

    public class TripSeatMapDto
    {
        public int TripId { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public List<SeatStatusDto> Seats { get; set; }
    }
}
