namespace Bus_Ticket_Booking.Mvc.Models.Auth
{
    public class LoginRequestModel
    {
        public string Email { get; set; }
            = string.Empty;

        public string Password { get; set; }
            = string.Empty;
    }
}