using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_Booking.Mvc.ViewModels.Driver
{
    public class CreateDriverViewModel
    {
        [Required]
        public string LicenseNumber { get; set; }
            = string.Empty;

        [Required]
        public string Name { get; set; }
            = string.Empty;

        [Required]
        public string Phone { get; set; }
            = string.Empty;

        public int OfficeId { get; set; }

        [Required]
        public string Address { get; set; }
            = string.Empty;

        [Required]
        public string City { get; set; }
            = string.Empty;

        [Required]
        public string State { get; set; }
            = string.Empty;

        [Required]
        public string ZipCode { get; set; }
            = string.Empty;
    }
}