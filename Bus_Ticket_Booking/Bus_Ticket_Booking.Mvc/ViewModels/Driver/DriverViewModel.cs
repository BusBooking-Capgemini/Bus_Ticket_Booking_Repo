namespace Bus_Ticket_Booking.Mvc.ViewModels.Driver
{
    public class DriverViewModel
    {
        public int DriverId { get; set; }

        public string LicenseNumber { get; set; }
            = string.Empty;

        public string Name { get; set; }
            = string.Empty;

        public string Phone { get; set; }
            = string.Empty;

        public int OfficeId { get; set; }

        public string OfficeName { get; set; }
            = string.Empty;

        public string Address { get; set; }
            = string.Empty;

        public string City { get; set; }
            = string.Empty;

        public string State { get; set; }
            = string.Empty;

        public string ZipCode { get; set; }
            = string.Empty;
    }
}