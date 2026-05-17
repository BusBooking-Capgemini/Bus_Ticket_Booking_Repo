namespace Bus_Ticket_Booking.Mvc.ViewModels.Trip
{
    public class TripViewModel
    {
        public int TripId { get; set; }

        public int RouteId { get; set; }

        public string FromCity { get; set; }
            = string.Empty;

        public string ToCity { get; set; }
            = string.Empty;

        // =========================
        // BUS
        // =========================

        public int BusId { get; set; }

        public string BusType { get; set; }
            = string.Empty;

        // =========================
        // ADDRESS IDS
        // =========================

        public int BoardingAddressId { get; set; }

        public int DroppingAddressId { get; set; }

        // =========================
        // ADDRESS NAMES
        // =========================

        public string BoardingCity { get; set; }
            = string.Empty;

        public string DroppingCity { get; set; }
            = string.Empty;

        // =========================
        // TIME
        // =========================

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        // =========================
        // DRIVER IDS
        // =========================

        public int Driver1DriverId { get; set; }

        public int Driver2DriverId { get; set; }

        // =========================
        // DRIVER NAMES
        // =========================

        public string Driver1Name { get; set; }
            = string.Empty;

        public string Driver2Name { get; set; }
            = string.Empty;

        // =========================
        // OTHER
        // =========================

        public int AvailableSeats { get; set; }

        public decimal Fare { get; set; }

        public DateTime TripDate { get; set; }
    }
}