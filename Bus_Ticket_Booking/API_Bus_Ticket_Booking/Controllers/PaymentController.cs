using API_Bus_Ticket_Booking.DTOs.Payment;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Create Payment
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment(
            [FromBody] CreatePaymentDto dto)
        {
            var result =
                await _paymentService.CreatePaymentAsync(dto);

            return Ok(result);
        }

        // Get Payment By Id
        [HttpGet("get-by-id/{paymentId}")]
        public async Task<IActionResult> GetPaymentById(int paymentId)
        {
            var result =
                await _paymentService.GetPaymentByIdAsync(paymentId);

            return Ok(result);
        }

        // Customer Payments
        [HttpGet("customer-payments/{customerId}")]
        public async Task<IActionResult> GetCustomerPayments(int customerId)
        {
            var result =
                await _paymentService.GetCustomerPaymentsAsync(customerId);

            return Ok(result);
        }

        // Office Payments
        [HttpGet("office-payments/{officeId}")]
        public async Task<IActionResult> GetOfficePayments(int officeId)
        {
            var result =
                await _paymentService.GetOfficePaymentsAsync(officeId);

            return Ok(result);
        }

        // Agency Payments
        [HttpGet("agency-payments/{agencyId}")]
        public async Task<IActionResult> GetAgencyPayments(int agencyId)
        {
            var result =
                await _paymentService.GetAgencyPaymentsAsync(agencyId);

            return Ok(result);
        }

        // Revenue Summary
        [HttpGet("revenue-summary")]
        public async Task<IActionResult> GetRevenueSummary(
            [FromQuery] int agencyId,
            [FromQuery] int? officeId)
        {
            var result =
                await _paymentService.GetRevenueSummaryAsync(
                    agencyId,
                    officeId);

            return Ok(result);
        }

        // Dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard(
            [FromQuery] int agencyId,
            [FromQuery] int? officeId)
        {
            var result =
                await _paymentService.GetDashboardAsync(
                    agencyId,
                    officeId);

            return Ok(result);
        }

        // Analytics
        [HttpGet("analytics")]
        public async Task<IActionResult> GetAnalytics(
            [FromQuery] int agencyId,
            [FromQuery] int? officeId)
        {
            var result =
                await _paymentService.GetAnalyticsAsync(
                    agencyId,
                    officeId);

            return Ok(result);
        }
    }
}