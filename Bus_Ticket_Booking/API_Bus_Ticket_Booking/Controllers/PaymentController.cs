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

        public PaymentController(
            IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Create Payment
        [HttpPost("create")]
        public async Task<IActionResult>
            CreatePayment(
                [FromBody] CreatePaymentDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message =
                        "Request body cannot be empty"
                });
            }

            var result =
                await _paymentService
                    .CreatePaymentAsync(dto);

            return Ok(new
            {
                success = true,
                message =
                    "Payment created successfully",
                data = result
            });
        }

        // Get Payment By Id
        [HttpGet("get-by-id/{paymentId}")]
        public async Task<IActionResult>
            GetPaymentById(int paymentId)
        {
            var result =
                await _paymentService
                    .GetPaymentByIdAsync(paymentId);

            if (result == null)
            {
                return Ok(new
                {
                    success = true,
                    message =
                        "Payment not found",
                    data = result
                });
            }

            return Ok(new
            {
                success = true,
                message =
                    "Payment fetched successfully",
                data = result
            });
        }

        // Customer Payments
        [HttpGet("customer-payments/{customerId}")]
        public async Task<IActionResult>
            GetCustomerPayments(int customerId)
        {
            var result =
                await _paymentService
                    .GetCustomerPaymentsAsync(
                        customerId);

            if (!result.Any())
            {
                return Ok(new
                {
                    success = true,
                    message =
                        "No payments found for this customer",
                    data = result
                });
            }

            return Ok(new
            {
                success = true,
                message =
                    "Customer payments fetched successfully",
                data = result
            });
        }

        // Office Payments
        [HttpGet("office-payments/{officeId}")]
        public async Task<IActionResult>
            GetOfficePayments(int officeId)
        {
            var result =
                await _paymentService
                    .GetOfficePaymentsAsync(
                        officeId);

            if (!result.Any())
            {
                return Ok(new
                {
                    success = true,
                    message =
                        "No payments found for this office",
                    data = result
                });
            }

            return Ok(new
            {
                success = true,
                message =
                    "Office payments fetched successfully",
                data = result
            });
        }

        // Agency Payments
        [HttpGet("agency-payments/{agencyId}")]
        public async Task<IActionResult>
            GetAgencyPayments(int agencyId)
        {
            var result =
                await _paymentService
                    .GetAgencyPaymentsAsync(
                        agencyId);

            if (!result.Any())
            {
                return Ok(new
                {
                    success = true,
                    message =
                        "No payments found for this agency",
                    data = result
                });
            }

            return Ok(new
            {
                success = true,
                message =
                    "Agency payments fetched successfully",
                data = result
            });
        }

        // Revenue Summary
        [HttpGet("revenue-summary")]
        public async Task<IActionResult>
            GetRevenueSummary(
                [FromQuery] int agencyId,
                [FromQuery] int? officeId)
        {
            var result =
                await _paymentService
                    .GetRevenueSummaryAsync(
                        agencyId,
                        officeId);

            return Ok(new
            {
                success = true,
                message =
                    "Revenue summary fetched successfully",
                data = result
            });
        }

        // Dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult>
            GetDashboard(
                [FromQuery] int agencyId,
                [FromQuery] int? officeId)
        {
            var result =
                await _paymentService
                    .GetDashboardAsync(
                        agencyId,
                        officeId);

            return Ok(new
            {
                success = true,
                message =
                    "Payment dashboard fetched successfully",
                data = result
            });
        }

        // Analytics
        [HttpGet("analytics")]
        public async Task<IActionResult>
            GetAnalytics(
                [FromQuery] int agencyId,
                [FromQuery] int? officeId)
        {
            var result =
                await _paymentService
                    .GetAnalyticsAsync(
                        agencyId,
                        officeId);

            return Ok(new
            {
                success = true,
                message =
                    "Payment analytics fetched successfully",
                data = result
            });
        }
    }
}