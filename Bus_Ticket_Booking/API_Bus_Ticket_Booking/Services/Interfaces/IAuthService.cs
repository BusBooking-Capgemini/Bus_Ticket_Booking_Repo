using API_Bus_Ticket_Booking.DTOs.Auth;

namespace API_Bus_Ticket_Booking.Services.Interfaces
{
    public interface IAuthService
    {
        // CUSTOMER

        Task<AuthResponseDto>
            CustomerSignupAsync(
                CustomerSignupDto dto);

        Task<AuthResponseDto>
            CustomerLoginAsync(
                LoginRequestDto dto);

        // AGENCY

        Task<AuthResponseDto>
            AgencyLoginAsync(
                LoginRequestDto dto);

        // OFFICE

        Task<AuthResponseDto>
            OfficeLoginAsync(
                LoginRequestDto dto);
    }
}