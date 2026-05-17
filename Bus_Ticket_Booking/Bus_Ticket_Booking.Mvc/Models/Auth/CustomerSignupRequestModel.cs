namespace Bus_Ticket_Booking.Mvc.Models.Auth
{
    public class CustomerSignupRequestModel
    {
        public string Name { get; set; }
            = string.Empty;

        public string Email { get; set; }
            = string.Empty;

        public string Phone { get; set; }
            = string.Empty;

        public string Password { get; set; }
            = string.Empty;
    }
}