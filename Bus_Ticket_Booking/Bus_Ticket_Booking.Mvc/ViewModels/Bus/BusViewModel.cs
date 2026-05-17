namespace Bus_Ticket_Booking.Mvc.ViewModels.Bus
{
    public class BusViewModel
    {
        public int BusId { get; set; }

        public string RegistrationNumber { get; set; }
            = string.Empty;

        public int Capacity { get; set; }

        public string Type { get; set; }
            = string.Empty;

        public int OfficeId { get; set; }
    }
}