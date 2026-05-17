using Bus_Ticket_Booking.Mvc.ViewModels.OfficePayment;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface IOfficePaymentService
    {
        Task<List<OfficePaymentViewModel>>
            GetOfficePaymentsAsync(
                string token);
    }
}