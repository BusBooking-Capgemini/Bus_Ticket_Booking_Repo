using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_Booking.Mvc.ViewModels.Bus
{
    public class CreateBusViewModel
    {
        [Required]
        public string RegistrationNumber { get; set; }
            = string.Empty;

        [Required]
        public int Capacity { get; set; }

        [Required]
        public string Type { get; set; }
            = string.Empty;

        public int OfficeId { get; set; }

        public List<SelectListItem>
            BusTypes
        { get; set; }
            = new();
    }
}