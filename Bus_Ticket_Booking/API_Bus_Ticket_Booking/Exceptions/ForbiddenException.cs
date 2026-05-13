namespace API_Bus_Ticket_Booking.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() { }

        public ForbiddenException(string message)
            : base(message) { }
    }
}
