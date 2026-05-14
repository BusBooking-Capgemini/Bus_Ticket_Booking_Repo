using API_Bus_Ticket_Booking.DTOs.Bus;

namespace TEST_Bus_Ticket_Booking.TestData
{
    public static class BusTestData
    {
        public static BusRequestDto GetBusRequestDto()
        {
            return new BusRequestDto
            {
                OfficeId = 1,
                RegistrationNumber = "PB10AA1111",
                Capacity = 40,
                Type = "AC"
            };
        }

        public static BusResponseDto GetBusResponseDto()
        {
            return new BusResponseDto
            {
                BusId = 1,
                OfficeId = 1,
                OfficeName = "Main Office",
                RegistrationNumber = "PB10AA1111",
                Capacity = 40,
                Type = "AC"
            };
        }

        public static List<BusResponseDto> GetBusResponseDtos()
        {
            return new List<BusResponseDto>
            {
                new BusResponseDto
                {
                    BusId = 1,
                    OfficeId = 1,
                    OfficeName = "Main Office",
                    RegistrationNumber = "PB10AA1111",
                    Capacity = 40,
                    Type = "AC"
                },
                new BusResponseDto
                {
                    BusId = 2,
                    OfficeId = 1,
                    OfficeName = "Main Office",
                    RegistrationNumber = "PB10AA2222",
                    Capacity = 45,
                    Type = "Non-AC"
                }
            };
        }
    }
}