using Bus_Ticket_Booking.Mvc.ViewModels.Customer;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerProfileViewModel?>
            GetProfileAsync(
                string token);

        Task<bool>
            UpdateProfileAsync(
                CustomerProfileViewModel model,
                string token);
    }
}