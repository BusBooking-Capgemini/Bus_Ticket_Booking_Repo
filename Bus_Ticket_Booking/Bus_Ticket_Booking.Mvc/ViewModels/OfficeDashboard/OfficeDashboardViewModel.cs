namespace Bus_Ticket_Booking.Mvc.ViewModels.OfficeDashboard
{
    public class OfficeDashboardViewModel
    {
      
        public int OfficeId { get; set; }

        public int TotalBuses { get; set; }

        public int TotalDrivers { get; set; }

        public int TotalBookings { get; set; }

        public int ActiveBookings { get; set; }

        public double OccupancyRate { get; set; }

        public int MostBookedTripId { get; set; }

        public string MostPopularRoute { get; set; }
            = string.Empty;

        public int TotalPayments { get; set; }

        public int SuccessfulPayments { get; set; }

        public int FailedPayments { get; set; }

        public decimal TotalRevenue { get; set; }

        public double SuccessRate { get; set; }

        public double FailureRate { get; set; }

        public decimal AveragePaymentAmount { get; set; }

        public string TopPayingRoute { get; set; }
            = string.Empty;
    }
}