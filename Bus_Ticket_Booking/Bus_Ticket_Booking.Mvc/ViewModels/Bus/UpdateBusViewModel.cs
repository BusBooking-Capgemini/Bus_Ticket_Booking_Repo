using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_Booking.Mvc.ViewModels.Bus
{
    public class UpdateBusViewModel
    {
        public int BusId
        { get; set; }


        [Required(
            ErrorMessage =
            "Registration number is required")]
        public string RegistrationNumber
        { get; set; }
            = string.Empty;


        [Required(
            ErrorMessage =
            "Capacity is required")]

        [Range(
            1,
            100,
            ErrorMessage =
            "Capacity must be between 1 and 100")]
        public int Capacity
        { get; set; }


        [Required(
            ErrorMessage =
            "Bus type is required")]
        public string Type
        { get; set; }
            = string.Empty;


        public int OfficeId
        { get; set; }



        public List<SelectListItem>
            BusTypes
        { get; set; }
            = new();
    }
}