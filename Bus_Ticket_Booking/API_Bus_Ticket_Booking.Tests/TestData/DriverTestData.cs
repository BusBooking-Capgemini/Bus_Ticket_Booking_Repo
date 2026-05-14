using API_Bus_Ticket_Booking.DTOs.Driver;

namespace TEST_Bus_Ticket_Booking.TestData
{
    public static class DriverTestData
    {
        public static DriverRequestDto GetDriverRequestDto()
        {
            return new DriverRequestDto
            {
                LicenseNumber = "LIC12345",
                Name = "Rohit Sharma",
                Phone = "9876543210",
                OfficeId = 1,
                Address = "Street 1",
                City = "Delhi",
                State = "Delhi",
                ZipCode = "110001"
            };
        }

        public static DriverResponseDto GetDriverResponseDto()
        {
            return new DriverResponseDto
            {
                DriverId = 1,
                LicenseNumber = "LIC12345",
                Name = "Rohit Sharma",
                Phone = "9876543210",
                OfficeId = 1,
                OfficeName = "Main Office",
                Address = "Street 1",
                City = "Delhi",
                State = "Delhi",
                ZipCode = "110001"
            };
        }

        public static List<DriverResponseDto> GetDriverResponseDtos()
        {
            return new List<DriverResponseDto>
            {
                GetDriverResponseDto(),
                new DriverResponseDto
                {
                    DriverId = 2,
                    LicenseNumber = "LIC67890",
                    Name = "Nikhil Verma",
                    Phone = "9876543211",
                    OfficeId = 1,
                    OfficeName = "Main Office",
                    Address = "Street 2",
                    City = "Delhi",
                    State = "Delhi",
                    ZipCode = "110002"
                }
            };
        }
    }
}