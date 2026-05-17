using Bus_Ticket_Booking.Mvc.ViewModels.OfficeBooking;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface IOfficeBookingService
    {
        Task<List<OfficeBookingViewModel>>
            GetOfficeBookingsAsync(
                string token);
    }
}