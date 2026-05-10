using API_Bus_Ticket_Booking.DTOs.Payment;

namespace API_Bus_Ticket_Booking.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreatePaymentAsync(CreatePaymentDto dto);

        Task<PaymentResponseDto> GetPaymentByIdAsync(int paymentId);

        Task<IEnumerable<PaymentResponseDto>> GetCustomerPaymentsAsync(int customerId);

        Task<IEnumerable<PaymentResponseDto>> GetOfficePaymentsAsync(int officeId);

        Task<IEnumerable<PaymentResponseDto>> GetAgencyPaymentsAsync(int agencyId);

        Task<RevenueSummaryDto> GetRevenueSummaryAsync(
            int agencyId,
            int? officeId);

        Task<PaymentDashboardDto> GetDashboardAsync(
            int agencyId,
            int? officeId);

        Task<PaymentAnalyticsDto> GetAnalyticsAsync(
            int agencyId,
            int? officeId);
    }
}