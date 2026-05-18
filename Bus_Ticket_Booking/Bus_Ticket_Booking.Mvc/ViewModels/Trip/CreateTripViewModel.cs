using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_Booking.Mvc.ViewModels.Trip
{
    public class CreateTripViewModel
    {
        [Required(ErrorMessage = "Route is required")]
        public int RouteId { get; set; }

        [Required(ErrorMessage = "Bus is required")]
        public int BusId { get; set; }

        [Required(ErrorMessage = "Boarding address is required")]
        public int BoardingAddressId { get; set; }

        [Required(ErrorMessage = "Dropping address is required")]
        public int DroppingAddressId { get; set; }

        [Required(ErrorMessage = "Departure time is required")]
        public DateTime DepartureTime { get; set; }

        [Required(ErrorMessage = "Arrival time is required")]
        public DateTime ArrivalTime { get; set; }

        [Required(ErrorMessage = "Driver 1 is required")]
        public int Driver1DriverId { get; set; }

        [Required(ErrorMessage = "Driver 2 is required")]
        public int Driver2DriverId { get; set; }

        [Required(ErrorMessage = "Fare is required")]

        [Range(1, 100000,
            ErrorMessage = "Fare must be greater than 0")]

        public decimal Fare { get; set; }

        [Required(ErrorMessage = "Trip date is required")]
        public DateTime TripDate { get; set; }

      

        public List<SelectListItem>
            Routes
        { get; set; }
            = new();

        public List<SelectListItem>
            Buses
        { get; set; }
            = new();

        public List<SelectListItem>
            Addresses
        { get; set; }
            = new();

        public List<SelectListItem>
            Drivers
        { get; set; }
            = new();
    }
}