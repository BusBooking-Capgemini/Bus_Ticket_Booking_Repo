namespace API_Bus_Ticket_Booking.Exceptions
{
    public class ValidationException:Exception
    {
        public ValidationException()
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }
    }
}
