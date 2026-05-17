namespace Bus_Ticket_Booking.Mvc.ViewModels.AgencyDashboard
{
    public class AgencyDashboardViewModel
    {
     

        public int AgencyId { get; set; }

        public string Name { get; set; }
            = string.Empty;

        public string ContactPersonName { get; set; }
            = string.Empty;

        public string Email { get; set; }
            = string.Empty;

        public string Phone { get; set; }
            = string.Empty;

      

        public int TotalOffices { get; set; }

     

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

    

        public List<AgencyOfficeCardViewModel>
            Offices
        { get; set; }
            = new();
    }


    public class AgencyOfficeCardViewModel
    {
        public int OfficeId { get; set; }

        public string OfficeMail { get; set; }
            = string.Empty;

        public string OfficeContactPersonName
        { get; set; }
            = string.Empty;

        public string OfficeContactNumber
        { get; set; }
            = string.Empty;
    }


    public class AgencyOfficeDetailsViewModel
    {
        public int OfficeId { get; set; }

        public int AgencyId { get; set; }

        public string OfficeMail { get; set; }
            = string.Empty;

        public string OfficeContactPersonName
        { get; set; }
            = string.Empty;

        public string OfficeContactNumber
        { get; set; }
            = string.Empty;

        public int OfficeAddressId
        { get; set; }


        public List<AgencyTripViewModel>
            Trips
        { get; set; }
            = new();

        public List<AgencyBookingViewModel>
            Bookings
        { get; set; }
            = new();

        public List<AgencyPaymentViewModel>
            Payments
        { get; set; }
            = new();

        public List<AgencyBusViewModel>
            Buses
        { get; set; }
            = new();

        public List<AgencyDriverViewModel>
            Drivers
        { get; set; }
            = new();
    }



    public class AgencyTripViewModel
    {
        public int TripId { get; set; }

        public string Route { get; set; }
            = string.Empty;

        public DateTime TripDate { get; set; }

        public DateTime DepartureTime
        { get; set; }

        public DateTime ArrivalTime
        { get; set; }

        public decimal Fare { get; set; }
    }



    public class AgencyBookingViewModel
    {
        public int BookingId { get; set; }

        public int TripId { get; set; }

        public int SeatNumber { get; set; }

        public string Status { get; set; }
            = string.Empty;
    }



    public class AgencyPaymentViewModel
    {
        public int PaymentId { get; set; }

        public int BookingId { get; set; }

        public decimal Amount { get; set; }

        public string Status
        { get; set; }
            = string.Empty;
    }


    public class AgencyBusViewModel
    {
        public int BusId { get; set; }

        public string RegistrationNumber
        { get; set; }
            = string.Empty;

        public int Capacity { get; set; }

        public string Type { get; set; }
            = string.Empty;
    }



    public class AgencyDriverViewModel
    {
        public int DriverId { get; set; }

        public string Name { get; set; }
            = string.Empty;

        public string Phone { get; set; }
            = string.Empty;

        public string LicenseNumber
        { get; set; }
            = string.Empty;
    }
}