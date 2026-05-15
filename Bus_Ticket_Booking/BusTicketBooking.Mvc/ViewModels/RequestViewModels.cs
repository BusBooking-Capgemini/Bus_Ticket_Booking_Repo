using System.ComponentModel.DataAnnotations;

namespace BusTicketBooking.Mvc.ViewModels;

public sealed class AgencyRequestViewModel
{
    [Required, StringLength(100)]
    public string? Name { get; set; }
    [Display(Name = "Contact Person"), StringLength(100)]
    public string? ContactPersonName { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    [Phone]
    public string? Phone { get; set; }
}

public sealed class BusRequestViewModel
{
    [Required, Display(Name = "Office")]
    public int OfficeId { get; set; }
    [Required, Display(Name = "Registration Number")]
    public string? RegistrationNumber { get; set; }
    [Range(1, 200)]
    public int Capacity { get; set; }
    [Required]
    public string? Type { get; set; }
}

public sealed class CreateBookingViewModel
{
    [Required, Display(Name = "Trip")]
    public int TripId { get; set; }
    [Required, Range(1, 200), Display(Name = "Seat Number")]
    public int SeatNumber { get; set; }
}

public sealed class CreatePaymentViewModel
{
    [Required, Display(Name = "Booking")]
    public int BookingId { get; set; }
    [Required, Display(Name = "Customer")]
    public int CustomerId { get; set; }
    [Required, Range(0.01, 1000000)]
    public double Amount { get; set; }
}

public sealed class CreateRouteViewModel
{
    [Required, Display(Name = "From City")]
    public string? FromCity { get; set; }
    [Required, Display(Name = "To City")]
    public string? ToCity { get; set; }
    [Display(Name = "Break Points")]
    public int? BreakPoints { get; set; }
    public int? Duration { get; set; }
}

public sealed class CustomerRequestViewModel
{
    [Required]
    public string? Name { get; set; }
    [Required, EmailAddress]
    public string? Email { get; set; }
    [Phone]
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    [Display(Name = "Zip Code")]
    public string? ZipCode { get; set; }
}

public sealed class DriverRequestViewModel
{
    [Required, Display(Name = "License Number")]
    public string? LicenseNumber { get; set; }
    [Required]
    public string? Name { get; set; }
    [Phone]
    public string? Phone { get; set; }
    [Required, Display(Name = "Office")]
    public int OfficeId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    [Display(Name = "Zip Code")]
    public string? ZipCode { get; set; }
}

public sealed class OfficeRequestViewModel
{
    [Required, Display(Name = "Agency")]
    public int AgencyId { get; set; }
    [Required, EmailAddress, Display(Name = "Office Email")]
    public string? OfficeMail { get; set; }
    [Display(Name = "Contact Person")]
    public string? OfficeContactPersonName { get; set; }
    [Phone, Display(Name = "Contact Number")]
    public string? OfficeContactNumber { get; set; }
    [Required, Display(Name = "Address")]
    public int OfficeAddressId { get; set; }
}

public sealed class ReviewRequestViewModel
{
    public int? ReviewId { get; set; }
    public int? TripId { get; set; }
    [Range(1, 5)]
    public int? Rating { get; set; }
    public string? Comment { get; set; }
}

public sealed class CreateTripViewModel
{
    [Required] public int RouteId { get; set; }
    [Required] public int BusId { get; set; }
    [Required] public int BoardingAddressId { get; set; }
    [Required] public int DroppingAddressId { get; set; }
    [Required] public DateTime DepartureTime { get; set; } = DateTime.Today.AddHours(8);
    [Required] public DateTime ArrivalTime { get; set; } = DateTime.Today.AddHours(12);
    [Required] public int Driver1DriverId { get; set; }
    [Required] public int Driver2DriverId { get; set; }
    [Required, Range(0.01, 1000000)] public double Fare { get; set; }
    [Required, DataType(DataType.Date)] public DateTime TripDate { get; set; } = DateTime.Today;
}
