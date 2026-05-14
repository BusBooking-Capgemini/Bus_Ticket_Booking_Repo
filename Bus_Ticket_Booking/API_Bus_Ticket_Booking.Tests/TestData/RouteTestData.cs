using API_Bus_Ticket_Booking.DTOs.Route;
using API_Bus_Ticket_Booking.DTOs.Trip;

namespace TEST_Bus_Ticket_Booking.TestData
{
    public static class RouteTestData
    {
        public static CreateRouteDto GetCreateRouteDto()
        {
            return new CreateRouteDto
            {
                FromCity = "Delhi",
                ToCity = "Mumbai",
                BreakPoints = 2,
                Duration = 720
            };
        }

        public static UpdateRouteDto GetUpdateRouteDto()
        {
            return new UpdateRouteDto
            {
                FromCity = "Delhi",
                ToCity = "Pune",
                BreakPoints = 1,
                Duration = 660
            };
        }

        public static RouteresponseDto GetRouteResponseDto()
        {
            return new RouteresponseDto
            {
                RouteId = 1,
                FromCity = "Delhi",
                ToCity = "Mumbai",
                BreakPoints = 2,
                Duration = 720
            };
        }

        public static List<RouteresponseDto> GetRouteResponseDtos()
        {
            return new List<RouteresponseDto>
            {
                GetRouteResponseDto(),
                new RouteresponseDto
                {
                    RouteId = 2,
                    FromCity = "Delhi",
                    ToCity = "Jaipur",
                    BreakPoints = 1,
                    Duration = 300
                }
            };
        }

        public static List<TripResponseDto> GetRouteTrips()
        {
            return new List<TripResponseDto>
            {
                new TripResponseDto
                {
                    TripId = 1,
                    RouteId = 1,
                    FromCity = "Delhi",
                    ToCity = "Mumbai",
                    BusId = 1,
                    BusType = "AC",
                    BoardingCity = "Delhi",
                    DroppingCity = "Mumbai",
                    DepartureTime = new DateTime(2026, 5, 15, 8, 0, 0),
                    ArrivalTime = new DateTime(2026, 5, 15, 20, 0, 0),
                    Driver1Name = "Rohit Sharma",
                    Driver2Name = "Nikhil Verma",
                    AvailableSeats = 12,
                    Fare = 1200,
                    TripDate = new DateTime(2026, 5, 15)
                }
            };
        }

        public static List<string> GetCities()
        {
            return new List<string>
            {
                "Delhi",
                "Mumbai",
                "Jaipur"
            };
        }
    }
}