using API_Bus_Ticket_Booking.DTOs.Trip;

namespace TEST_Bus_Ticket_Booking.TestData
{
    public static class TripTestData
    {
        public static CreateTripDto GetCreateTripDto()
        {
            return new CreateTripDto
            {
                RouteId = 1,
                BusId = 1,
                BoardingAddressId = 1,
                DroppingAddressId = 2,
                DepartureTime = new DateTime(2026, 5, 15, 8, 0, 0),
                ArrivalTime = new DateTime(2026, 5, 15, 20, 0, 0),
                Driver1DriverId = 1,
                Driver2DriverId = 2,
                Fare = 1200,
                TripDate = new DateTime(2026, 5, 15)
            };
        }

        public static UpdateTripDto GetUpdateTripDto()
        {
            return new UpdateTripDto
            {
                BusId = 1,
                BoardingAddressId = 1,
                DroppingAddressId = 2,
                DepartureTime = new DateTime(2026, 5, 15, 8, 0, 0),
                ArrivalTime = new DateTime(2026, 5, 15, 20, 0, 0),
                Driver1DriverId = 1,
                Driver2DriverId = 2,
                Fare = 1300
            };
        }

        public static TripSearchDto GetTripSearchDto()
        {
            return new TripSearchDto
            {
                FromCity = "Delhi",
                ToCity = "Mumbai",
                TripDate = new DateTime(2026, 5, 15)
            };
        }

        public static TripResponseDto GetTripResponseDto()
        {
            return new TripResponseDto
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
            };
        }

        public static List<TripResponseDto> GetTripResponseDtos()
        {
            return new List<TripResponseDto>
            {
                GetTripResponseDto(),
                new TripResponseDto
                {
                    TripId = 2,
                    RouteId = 2,
                    FromCity = "Delhi",
                    ToCity = "Jaipur",
                    BusId = 2,
                    BusType = "Non-AC",
                    BoardingCity = "Delhi",
                    DroppingCity = "Jaipur",
                    DepartureTime = new DateTime(2026, 5, 16, 9, 0, 0),
                    ArrivalTime = new DateTime(2026, 5, 16, 15, 0, 0),
                    Driver1Name = "Amit Sharma",
                    Driver2Name = "Rahul Kumar",
                    AvailableSeats = 18,
                    Fare = 800,
                    TripDate = new DateTime(2026, 5, 16)
                }
            };
        }

        public static TripSeatMapDto GetTripSeatMapDto()
        {
            return new TripSeatMapDto
            {
                TripId = 1,
                TotalSeats = 40,
                AvailableSeats = 12,
                Seats = new List<SeatStatusDto>
                {
                    new SeatStatusDto
                    {
                        SeatNumber = 1,
                        Status = "Booked"
                    },
                    new SeatStatusDto
                    {
                        SeatNumber = 2,
                        Status = "Available"
                    }
                }
            };
        }
    }
}