using API_Bus_Ticket_Booking.DTOs.Payment;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        private readonly IMapper _mapper;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;

            _mapper = mapper;
        }

        // Create Payment
        public async Task<PaymentResponseDto>
            CreatePaymentAsync(
                CreatePaymentDto dto)
        {
            var existingPayment =
                await _paymentRepository
                    .GetPaymentByBookingIdAsync(
                        dto.BookingId);

            if (existingPayment != null)
            {
                throw new ConflictException(
                    "Payment already exists for this booking");
            }

            var payment = new Payment
            {
                BookingId = dto.BookingId,

                CustomerId = dto.CustomerId,

                Amount = dto.Amount,

                PaymentDate = DateTime.Now,

                PaymentStatus = dto.Amount > 0
                    ? "Success"
                    : "Failed"
            };

            var createdPayment =
                await _paymentRepository
                    .CreatePaymentAsync(payment);

            return _mapper.Map<
                PaymentResponseDto>(
                    createdPayment);
        }

        // Get Payment By Id
        public async Task<PaymentResponseDto>
            GetPaymentByIdAsync(
                int paymentId)
        {
            var payment =
                await _paymentRepository
                    .GetPaymentByIdAsync(
                        paymentId);

            if (payment == null)
            {
                return null!;
            }

            return _mapper.Map<
                PaymentResponseDto>(
                    payment);
        }

        // Customer Payments
        public async Task<
            IEnumerable<PaymentResponseDto>>
            GetCustomerPaymentsAsync(
                int customerId)
        {
            var payments =
                await _paymentRepository
                    .GetCustomerPaymentsAsync(
                        customerId);

            return _mapper.Map<
                IEnumerable<PaymentResponseDto>>(
                    payments);
        }

        // Office Payments
        public async Task<
            IEnumerable<PaymentResponseDto>>
            GetOfficePaymentsAsync(
                int officeId)
        {
            var payments =
                await _paymentRepository
                    .GetOfficePaymentsAsync(
                        officeId);

            return _mapper.Map<
                IEnumerable<PaymentResponseDto>>(
                    payments);
        }

        // Agency Payments
        public async Task<
            IEnumerable<PaymentResponseDto>>
            GetAgencyPaymentsAsync(
                int agencyId)
        {
            var payments =
                await _paymentRepository
                    .GetAgencyPaymentsAsync(
                        agencyId);

            return _mapper.Map<
                IEnumerable<PaymentResponseDto>>(
                    payments);
        }

        // Revenue Summary
        public async Task<RevenueSummaryDto>
            GetRevenueSummaryAsync(
                int agencyId,
                int? officeId)
        {
            if (officeId.HasValue)
            {
                return new RevenueSummaryDto
                {
                    TotalRevenue =
                        await _paymentRepository
                            .GetTotalRevenueByOfficeAsync(
                                officeId.Value),

                    TodayRevenue =
                        await _paymentRepository
                            .GetTodayRevenueByOfficeAsync(
                                officeId.Value),

                    MonthlyRevenue =
                        await _paymentRepository
                            .GetMonthlyRevenueByOfficeAsync(
                                officeId.Value)
                };
            }

            return new RevenueSummaryDto
            {
                TotalRevenue =
                    await _paymentRepository
                        .GetTotalRevenueByAgencyAsync(
                            agencyId),

                TodayRevenue =
                    await _paymentRepository
                        .GetTodayRevenueByAgencyAsync(
                            agencyId),

                MonthlyRevenue =
                    await _paymentRepository
                        .GetMonthlyRevenueByAgencyAsync(
                            agencyId)
            };
        }

        // Dashboard
        public async Task<PaymentDashboardDto>
            GetDashboardAsync(
                int agencyId,
                int? officeId)
        {
            if (officeId.HasValue)
            {
                var successful =
                    await _paymentRepository
                        .GetSuccessfulPaymentsByOfficeAsync(
                            officeId.Value);

                var failed =
                    await _paymentRepository
                        .GetFailedPaymentsByOfficeAsync(
                            officeId.Value);

                return new PaymentDashboardDto
                {
                    SuccessfulPayments = successful,

                    FailedPayments = failed,

                    TotalRevenue =
                        await _paymentRepository
                            .GetTotalRevenueByOfficeAsync(
                                officeId.Value),

                    TotalPayments =
                        successful + failed
                };
            }

            var agencySuccessful =
                await _paymentRepository
                    .GetSuccessfulPaymentsByAgencyAsync(
                        agencyId);

            var agencyFailed =
                await _paymentRepository
                    .GetFailedPaymentsByAgencyAsync(
                        agencyId);

            return new PaymentDashboardDto
            {
                SuccessfulPayments =
                    agencySuccessful,

                FailedPayments =
                    agencyFailed,

                TotalRevenue =
                    await _paymentRepository
                        .GetTotalRevenueByAgencyAsync(
                            agencyId),

                TotalPayments =
                    agencySuccessful + agencyFailed
            };
        }

        // Analytics
        public async Task<PaymentAnalyticsDto>
            GetAnalyticsAsync(
                int agencyId,
                int? officeId)
        {
            int successfulPayments;

            int failedPayments;

            decimal averageAmount;

            string topRoute;

            if (officeId.HasValue)
            {
                successfulPayments =
                    await _paymentRepository
                        .GetSuccessfulPaymentsByOfficeAsync(
                            officeId.Value);

                failedPayments =
                    await _paymentRepository
                        .GetFailedPaymentsByOfficeAsync(
                            officeId.Value);

                averageAmount =
                    await _paymentRepository
                        .GetAveragePaymentAmountByOfficeAsync(
                            officeId.Value);

                topRoute =
                    await _paymentRepository
                        .GetTopPayingRouteByOfficeAsync(
                            officeId.Value);
            }
            else
            {
                successfulPayments =
                    await _paymentRepository
                        .GetSuccessfulPaymentsByAgencyAsync(
                            agencyId);

                failedPayments =
                    await _paymentRepository
                        .GetFailedPaymentsByAgencyAsync(
                            agencyId);

                averageAmount =
                    await _paymentRepository
                        .GetAveragePaymentAmountByAgencyAsync(
                            agencyId);

                topRoute =
                    await _paymentRepository
                        .GetTopPayingRouteByAgencyAsync(
                            agencyId);
            }

            int totalPayments =
                successfulPayments + failedPayments;

            double successRate = 0;

            double failureRate = 0;

            if (totalPayments > 0)
            {
                successRate =
                    ((double)successfulPayments /
                    totalPayments) * 100;

                failureRate =
                    ((double)failedPayments /
                    totalPayments) * 100;
            }

            return new PaymentAnalyticsDto
            {
                SuccessRate = successRate,

                FailureRate = failureRate,

                AveragePaymentAmount =
                    averageAmount,

                TopPayingRoute =
                    topRoute
            };
        }
    }
}