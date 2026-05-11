namespace API_Bus_Ticket_Booking.DTOs.Auth
{
    // LOGIN
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    // CUSTOMER SIGNUP
    public class CustomerSignupDto
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    // AUTH RESPONSE
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}