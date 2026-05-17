using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_Booking.Mvc.ViewModels.Trip
{
    public class UpdateTripViewModel
    {
        public int TripId { get; set; }

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


        // =========================
        // DROPDOWNS
        // =========================

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