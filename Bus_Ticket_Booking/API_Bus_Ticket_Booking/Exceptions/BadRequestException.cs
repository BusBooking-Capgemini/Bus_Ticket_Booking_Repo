namespace API_Bus_Ticket_Booking.Exceptions
{
    public class BadRequestException: Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
