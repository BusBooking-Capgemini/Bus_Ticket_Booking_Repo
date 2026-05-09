namespace API_Bus_Ticket_Booking.Exceptions
{
    public class UnauthorizedException:Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}
