using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_Booking.Mvc.ViewModels.Driver
{
    public class CreateDriverViewModel
    {
        [Required(
            ErrorMessage =
            "License number is required")]
        public string LicenseNumber
        { get; set; }
            = string.Empty;


        [Required(
            ErrorMessage =
            "Driver name is required")]

        [MinLength(
            3,
            ErrorMessage =
            "Name must be at least 3 characters")]
        public string Name
        { get; set; }
            = string.Empty;


        [Required(
            ErrorMessage =
            "Phone number is required")]

        [RegularExpression(
            @"^[0-9]{10}$",
            ErrorMessage =
            "Phone number must be 10 digits")]
        public string Phone
        { get; set; }
            = string.Empty;


        public int OfficeId
        { get; set; }


        [Required(
            ErrorMessage =
            "Address is required")]
        public string Address
        { get; set; }
            = string.Empty;


        [Required(
            ErrorMessage =
            "City is required")]
        public string City
        { get; set; }
            = string.Empty;


        [Required(
            ErrorMessage =
            "State is required")]
        public string State
        { get; set; }
            = string.Empty;


        [Required(
            ErrorMessage =
            "Zip code is required")]

        [RegularExpression(
            @"^[0-9]{6}$",
            ErrorMessage =
            "Zip code must be 6 digits")]
        public string ZipCode
        { get; set; }
            = string.Empty;
    }
}