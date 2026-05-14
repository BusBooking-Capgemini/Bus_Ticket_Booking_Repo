using API_Bus_Ticket_Booking.DTOs.Auth;

namespace TEST_Bus_Ticket_Booking.TestData
{
    public static class AuthTestData
    {
        public static CustomerSignupDto GetCustomerSignupDto()
        {
            return new CustomerSignupDto
            {
                Name = "Aarav Sharma",
                Email = "aarav@test.com",
                Phone = "9876543210",
                Password = "Password@123"
            };
        }

        public static LoginRequestDto GetCustomerLoginDto()
        {
            return new LoginRequestDto
            {
                Email = "customer@test.com",
                Password = "Password@123"
            };
        }

        public static LoginRequestDto GetAgencyLoginDto()
        {
            return new LoginRequestDto
            {
                Email = "agency@test.com",
                Password = "Password@123"
            };
        }

        public static LoginRequestDto GetOfficeLoginDto()
        {
            return new LoginRequestDto
            {
                Email = "office@test.com",
                Password = "Password@123"
            };
        }

        public static AuthResponseDto GetCustomerAuthResponse()
        {
            return new AuthResponseDto
            {
                Token = "customer-token",
                Role = "Customer",
                Email = "customer@test.com"
            };
        }

        public static AuthResponseDto GetAgencyAuthResponse()
        {
            return new AuthResponseDto
            {
                Token = "agency-token",
                Role = "Agency",
                Email = "agency@test.com"
            };
        }

        public static AuthResponseDto GetOfficeAuthResponse()
        {
            return new AuthResponseDto
            {
                Token = "office-token",
                Role = "Office",
                Email = "office@test.com"
            };
        }
    }
}