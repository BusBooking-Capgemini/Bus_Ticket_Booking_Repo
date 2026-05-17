using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_Booking.Mvc.ViewModels.Customer
{
    public class CustomerProfileViewModel
    {
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }
            = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; }
            = string.Empty;

        [Required]
        public string Phone { get; set; }
            = string.Empty;

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }
    }
}