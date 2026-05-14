using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.DTOs.Booking;
using API_Bus_Ticket_Booking.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TEST_Bus_Ticket_Booking.Helpers;
using TEST_Bus_Ticket_Booking.TestData;

namespace TEST_Bus_Ticket_Booking.Controllers
{
    public class BookingControllerTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly BookingController _controller;

        public BookingControllerTests()
        {
            _bookingServiceMock = new Mock<IBookingService>();
            _controller = new BookingController(_bookingServiceMock.Object);
        }

        // POSITIVE TEST CASES

        [Fact]
        public async Task CreateBooking_ShouldReturnSuccessResponse()
        {
            var dto = BookingTestData.GetCreateBookingDto();
            var response = BookingTestData.GetBookingResponse();

            _bookingServiceMock.Setup(x => x.CreateBookingAsync(dto)).ReturnsAsync(response);

            var result = await _controller.CreateBooking(dto);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            TestHelper.GetPropertyValue<string>(okResult.Value!, "message").Should().Be("Booking created successfully");
        }

        [Fact]
        public async Task GetBookingById_ShouldReturnBooking()
        {
            var response = BookingTestData.GetBookingResponse();

            _bookingServiceMock.Setup(x => x.GetBookingByIdAsync(1)).ReturnsAsync(response);

            var result = await _controller.GetBookingById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Booking fetched successfully");
        }

        [Fact]
        public async Task GetCustomerBookings_ShouldReturnBookings()
        {
            var bookings = BookingTestData.GetBookings();

            _bookingServiceMock.Setup(x => x.GetCustomerBookingsAsync(1)).ReturnsAsync(bookings);

            var result = await _controller.GetCustomerBookings(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Customer bookings fetched successfully");
            TestHelper.GetPropertyValue<IEnumerable<BookingResponseDto>>(okResult.Value!, "data")!.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetDashboard_ShouldReturnDashboard()
        {
            var dashboard = BookingTestData.GetDashboard();

            _bookingServiceMock.Setup(x => x.GetDashboardAsync(1, null)).ReturnsAsync(dashboard);

            var result = await _controller.GetDashboard(1, null);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Dashboard fetched successfully");
        }

        // NEGATIVE TEST CASES

        [Fact]
        public async Task CreateBooking_ShouldReturnBadRequest_WhenBodyIsNull()
        {
            var result = await _controller.CreateBooking(null!);

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(badRequestResult!.Value!, "message").Should().Be("Request body cannot be empty");
        }

        [Fact]
        public async Task CancelBooking_ShouldThrowException()
        {
            _bookingServiceMock.Setup(x => x.CancelBookingAsync(1)).ThrowsAsync(new Exception("Cancel failed"));

            Func<Task> action = async () => await _controller.CancelBooking(1);

            await action.Should().ThrowAsync<Exception>().WithMessage("Cancel failed");
        }

        [Fact]
        public async Task GetBookingById_ShouldReturnBookingNotFoundMessage()
        {
            _bookingServiceMock.Setup(x => x.GetBookingByIdAsync(1)).ReturnsAsync((BookingResponseDto?)null);

            var result = await _controller.GetBookingById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Booking not found");
        }

        [Fact]
        public async Task GetCustomerBookings_ShouldReturnNoBookingsMessage_WhenEmpty()
        {
            _bookingServiceMock.Setup(x => x.GetCustomerBookingsAsync(1)).ReturnsAsync(Array.Empty<BookingResponseDto>());

            var result = await _controller.GetCustomerBookings(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("No bookings found for this customer");
        }
    }
}