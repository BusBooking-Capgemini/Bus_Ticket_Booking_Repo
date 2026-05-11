using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly BusTicketBookingContext _context;

        public PaymentRepository(BusTicketBookingContext context)
        {
            _context = context;
        }

        // Create Payment
        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);

            await SaveChangesAsync();

            return payment;
        }

        // Get Payment By Id
        public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Booking)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        // Customer Payments
        public async Task<IEnumerable<Payment>> GetCustomerPaymentsAsync(int customerId)
        {
            return await _context.Payments
                .Where(p => p.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync();
        }

        // Office Payments
        public async Task<IEnumerable<Payment>> GetOfficePaymentsAsync(int officeId)
        {
            return await _context.Payments
                .Where(p =>
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.OfficeId == officeId)
                .AsNoTracking()
                .ToListAsync();
        }

        // Agency Payments
        public async Task<IEnumerable<Payment>> GetAgencyPaymentsAsync(int agencyId)
        {
            return await _context.Payments
                .Where(p =>
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.Office != null &&
                    p.Booking.Trip.Bus.Office.AgencyId == agencyId)
                .AsNoTracking()
                .ToListAsync();
        }

        // Payment By Booking
        public async Task<Payment?> GetPaymentByBookingIdAsync(int bookingId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);
        }

        // GLOBAL REVENUE

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Payments
                .Where(p => p.PaymentStatus == "Success")
                .SumAsync(p => p.Amount ?? 0);
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            var today = DateTime.Today;

            return await _context.Payments
                .Where(p =>
                    p.PaymentDate.HasValue &&
                    p.PaymentDate.Value.Date == today &&
                    p.PaymentStatus == "Success")
                .SumAsync(p => p.Amount ?? 0);
        }

        public async Task<decimal> GetMonthlyRevenueAsync()
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            return await _context.Payments
                .Where(p =>
                    p.PaymentDate.HasValue &&
                    p.PaymentDate.Value.Month == currentMonth &&
                    p.PaymentDate.Value.Year == currentYear &&
                    p.PaymentStatus == "Success")
                .SumAsync(p => p.Amount ?? 0);
        }

        // OFFICE REVENUE

        public async Task<decimal> GetTotalRevenueByOfficeAsync(int officeId)
        {
            return await _context.Payments
                .Where(p =>
                    p.PaymentStatus == "Success" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.OfficeId == officeId)
                .SumAsync(p => p.Amount ?? 0);
        }

        public async Task<decimal> GetTodayRevenueByOfficeAsync(int officeId)
        {
            var today = DateTime.Today;

            return await _context.Payments
                .Where(p =>
                    p.PaymentDate.HasValue &&
                    p.PaymentDate.Value.Date == today &&
                    p.PaymentStatus == "Success" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.OfficeId == officeId)
                .SumAsync(p => p.Amount ?? 0);
        }

        public async Task<decimal> GetMonthlyRevenueByOfficeAsync(int officeId)
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            return await _context.Payments
                .Where(p =>
                    p.PaymentDate.HasValue &&
                    p.PaymentDate.Value.Month == currentMonth &&
                    p.PaymentDate.Value.Year == currentYear &&
                    p.PaymentStatus == "Success" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.OfficeId == officeId)
                .SumAsync(p => p.Amount ?? 0);
        }

        // AGENCY REVENUE

        public async Task<decimal> GetTotalRevenueByAgencyAsync(int agencyId)
        {
            return await _context.Payments
                .Where(p =>
                    p.PaymentStatus == "Success" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.Office != null &&
                    p.Booking.Trip.Bus.Office.AgencyId == agencyId)
                .SumAsync(p => p.Amount ?? 0);
        }

        public async Task<decimal> GetTodayRevenueByAgencyAsync(int agencyId)
        {
            var today = DateTime.Today;

            return await _context.Payments
                .Where(p =>
                    p.PaymentDate.HasValue &&
                    p.PaymentDate.Value.Date == today &&
                    p.PaymentStatus == "Success" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.Office != null &&
                    p.Booking.Trip.Bus.Office.AgencyId == agencyId)
                .SumAsync(p => p.Amount ?? 0);
        }

        public async Task<decimal> GetMonthlyRevenueByAgencyAsync(int agencyId)
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            return await _context.Payments
                .Where(p =>
                    p.PaymentDate.HasValue &&
                    p.PaymentDate.Value.Month == currentMonth &&
                    p.PaymentDate.Value.Year == currentYear &&
                    p.PaymentStatus == "Success" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.Office != null &&
                    p.Booking.Trip.Bus.Office.AgencyId == agencyId)
                .SumAsync(p => p.Amount ?? 0);
        }

        // GLOBAL PAYMENT COUNTS

        public async Task<int> GetSuccessfulPaymentsCountAsync()
        {
            return await _context.Payments
                .CountAsync(p => p.PaymentStatus == "Success");
        }

        public async Task<int> GetFailedPaymentsCountAsync()
        {
            return await _context.Payments
                .CountAsync(p => p.PaymentStatus == "Failed");
        }

        // OFFICE PAYMENT COUNTS

        public async Task<int> GetSuccessfulPaymentsByOfficeAsync(int officeId)
        {
            return await _context.Payments
                .CountAsync(p =>
                    p.PaymentStatus == "Success" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.OfficeId == officeId);
        }

        public async Task<int> GetFailedPaymentsByOfficeAsync(int officeId)
        {
            return await _context.Payments
                .CountAsync(p =>
                    p.PaymentStatus == "Failed" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.OfficeId == officeId);
        }

        // AGENCY PAYMENT COUNTS

        public async Task<int> GetSuccessfulPaymentsByAgencyAsync(int agencyId)
        {
            return await _context.Payments
                .CountAsync(p =>
                    p.PaymentStatus == "Success" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.Office != null &&
                    p.Booking.Trip.Bus.Office.AgencyId == agencyId);
        }

        public async Task<int> GetFailedPaymentsByAgencyAsync(int agencyId)
        {
            return await _context.Payments
                .CountAsync(p =>
                    p.PaymentStatus == "Failed" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.Office != null &&
                    p.Booking.Trip.Bus.Office.AgencyId == agencyId);
        }

        // ANALYTICS

        public async Task<decimal> GetAveragePaymentAmountByOfficeAsync(int officeId)
        {
            return await _context.Payments
                .Where(p =>
                    p.PaymentStatus == "Success" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.OfficeId == officeId)
                .AverageAsync(p => p.Amount ?? 0);
        }

        public async Task<decimal> GetAveragePaymentAmountByAgencyAsync(int agencyId)
        {
            return await _context.Payments
                .Where(p =>
                    p.PaymentStatus == "Success" &&
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.Office != null &&
                    p.Booking.Trip.Bus.Office.AgencyId == agencyId)
                .AverageAsync(p => p.Amount ?? 0);
        }

        public async Task<string> GetTopPayingRouteByOfficeAsync(int officeId)
        {
            var route = await _context.Payments
                .Where(p =>
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Route != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.OfficeId == officeId)
                .GroupBy(p =>
                    p.Booking!.Trip!.Route!.FromCity + " - " +
                    p.Booking.Trip.Route.ToCity)
                .OrderByDescending(g => g.Sum(x => x.Amount ?? 0))
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return route ?? string.Empty;
        }

        public async Task<string> GetTopPayingRouteByAgencyAsync(int agencyId)
        {
            var route = await _context.Payments
                .Where(p =>
                    p.Booking != null &&
                    p.Booking.Trip != null &&
                    p.Booking.Trip.Route != null &&
                    p.Booking.Trip.Bus != null &&
                    p.Booking.Trip.Bus.Office != null &&
                    p.Booking.Trip.Bus.Office.AgencyId == agencyId)
                .GroupBy(p =>
                    p.Booking!.Trip!.Route!.FromCity + " - " +
                    p.Booking.Trip.Route.ToCity)
                .OrderByDescending(g => g.Sum(x => x.Amount ?? 0))
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return route ?? string.Empty;
        }

        // Save Changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}