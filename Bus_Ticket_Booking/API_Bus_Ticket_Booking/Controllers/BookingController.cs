using API_Bus_Ticket_Booking.DTOs.Booking;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // Create Booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            [FromBody] CreateBookingDto dto)
        {
            var result =
                await _bookingService.CreateBookingAsync(dto);

            return CreatedAtAction(
                nameof(GetBookingById),
                new { bookingId = result.BookingId },
                result);
        }

        // Cancel Booking
        [HttpPut("{bookingId}/cancel")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            await _bookingService.CancelBookingAsync(bookingId);

            return Ok(new
            {
                message = "Booking cancelled successfully"
            });
        }

        // Get Booking By Id
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            var result =
                await _bookingService.GetBookingByIdAsync(bookingId);

            return Ok(result);
        }

        // Customer Bookings
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerBookings(int customerId)
        {
            var result =
                await _bookingService.GetCustomerBookingsAsync(customerId);

            return Ok(result);
        }

        // Office Bookings
        [HttpGet("office/{officeId}")]
        public async Task<IActionResult> GetOfficeBookings(int officeId)
        {
            var result =
                await _bookingService.GetOfficeBookingsAsync(officeId);

            return Ok(result);
        }

        // Agency Bookings
        [HttpGet("agency/{agencyId}")]
        public async Task<IActionResult> GetAgencyBookings(int agencyId)
        {
            var result =
                await _bookingService.GetAgencyBookingsAsync(agencyId);

            return Ok(result);
        }

        // Dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard(
            [FromQuery] int agencyId,
            [FromQuery] int? officeId)
        {
            var result =
                await _bookingService.GetDashboardAsync(
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
                await _bookingService.GetAnalyticsAsync(
                    agencyId,
                    officeId);

            return Ok(result);
        }
    }
}