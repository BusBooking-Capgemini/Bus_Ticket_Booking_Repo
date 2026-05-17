using Bus_Ticket_Booking.Mvc.ViewModels.Payment;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool>
            ProcessPaymentAsync(
                PaymentViewModel model);

        Task<bool>
            CreatePaymentAsync(
                int bookingId,
                decimal amount,
                string token);

        Task<List<PaymentListViewModel>>
            GetMyPaymentsAsync(
                string token);
    }
}