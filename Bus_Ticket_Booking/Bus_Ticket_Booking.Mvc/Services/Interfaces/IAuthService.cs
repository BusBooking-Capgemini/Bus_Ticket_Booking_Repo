using Bus_Ticket_Booking.Mvc.ViewModels.Auth;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseViewModel?>
            CustomerLoginAsync(
                LoginViewModel model);

        Task<AuthResponseViewModel?>
    AgencyLoginAsync(
        AgencyLoginViewModel model);

        Task<AuthResponseViewModel?>
            OfficeLoginAsync(
                LoginViewModel model);

        Task<bool>
            CustomerSignupAsync(
                RegisterViewModel model);
    }
}