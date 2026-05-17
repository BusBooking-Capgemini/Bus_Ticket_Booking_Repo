namespace Bus_Ticket_Booking.Mvc.ViewModels.Auth
{
    public class AuthResponseViewModel
    {
        public string Token { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}