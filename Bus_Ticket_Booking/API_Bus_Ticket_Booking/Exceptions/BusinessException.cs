namespace API_Bus_Ticket_Booking.Exceptions
{
    public class BusinessException:Exception
    {
        public BusinessException()
        {
        }

        public BusinessException(string message)
            : base(message)
        {
        }
    }
}
