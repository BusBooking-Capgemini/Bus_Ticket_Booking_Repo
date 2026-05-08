namespace API_Bus_Ticket_Booking.Exception
{
    public class BadRequestException : System.Exception
    {
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
