using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        // Create Payment
        Task<Payment> CreatePaymentAsync(Payment payment);

        // Get Payment By Id
        Task<Payment?> GetPaymentByIdAsync(int paymentId);

        // Customer Payments
        Task<IEnumerable<Payment>> GetCustomerPaymentsAsync(int customerId);

        // Office Payments
        Task<IEnumerable<Payment>> GetOfficePaymentsAsync(int officeId);

        // Agency Payments
        Task<IEnumerable<Payment>> GetAgencyPaymentsAsync(int agencyId);

        // Payment By Booking
        Task<Payment?> GetPaymentByBookingIdAsync(int bookingId);
        
        Task<decimal> GetTotalRevenueAsync();

        Task<decimal> GetTodayRevenueAsync();

        Task<decimal> GetMonthlyRevenueAsync();

        Task<decimal> GetTotalRevenueByOfficeAsync(int officeId);

        Task<decimal> GetTodayRevenueByOfficeAsync(int officeId);

        Task<decimal> GetMonthlyRevenueByOfficeAsync(int officeId);

     

        Task<decimal> GetTotalRevenueByAgencyAsync(int agencyId);

        Task<decimal> GetTodayRevenueByAgencyAsync(int agencyId);

        Task<decimal> GetMonthlyRevenueByAgencyAsync(int agencyId);


        Task<int> GetSuccessfulPaymentsCountAsync();

        Task<int> GetFailedPaymentsCountAsync();


        Task<int> GetSuccessfulPaymentsByOfficeAsync(int officeId);

        Task<int> GetFailedPaymentsByOfficeAsync(int officeId);


        Task<int> GetSuccessfulPaymentsByAgencyAsync(int agencyId);

        Task<int> GetFailedPaymentsByAgencyAsync(int agencyId);


        Task<decimal> GetAveragePaymentAmountByOfficeAsync(int officeId);

        Task<decimal> GetAveragePaymentAmountByAgencyAsync(int agencyId);

        Task<string> GetTopPayingRouteByOfficeAsync(int officeId);

        Task<string> GetTopPayingRouteByAgencyAsync(int agencyId);

        // Save Changes
        Task SaveChangesAsync();
    }
}