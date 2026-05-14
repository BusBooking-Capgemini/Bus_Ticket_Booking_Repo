using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.DTOs.Payment;
using API_Bus_Ticket_Booking.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TEST_Bus_Ticket_Booking.Helpers;
using TEST_Bus_Ticket_Booking.TestData;

namespace TEST_Bus_Ticket_Booking.Controllers
{
    public class PaymentControllerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly PaymentController _controller;

        public PaymentControllerTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _controller = new PaymentController(_paymentServiceMock.Object);
        }

        // POSITIVE TEST CASES

        [Fact]
        public async Task CreatePayment_ShouldReturnSuccessResponse()
        {
            var dto = PaymentTestData.GetCreatePaymentDto();
            var response = PaymentTestData.GetPaymentResponse();

            _paymentServiceMock.Setup(x => x.CreatePaymentAsync(dto)).ReturnsAsync(response);

            var result = await _controller.CreatePayment(dto);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            TestHelper.GetPropertyValue<string>(okResult.Value!, "message").Should().Be("Payment created successfully");
        }

        [Fact]
        public async Task GetPaymentById_ShouldReturnPayment()
        {
            var response = PaymentTestData.GetPaymentResponse();

            _paymentServiceMock.Setup(x => x.GetPaymentByIdAsync(1)).ReturnsAsync(response);

            var result = await _controller.GetPaymentById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Payment fetched successfully");
        }

        [Fact]
        public async Task GetCustomerPayments_ShouldReturnPayments()
        {
            var payments = PaymentTestData.GetPayments();

            _paymentServiceMock.Setup(x => x.GetCustomerPaymentsAsync(1)).ReturnsAsync(payments);

            var result = await _controller.GetCustomerPayments(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Customer payments fetched successfully");
            TestHelper.GetPropertyValue<IEnumerable<PaymentResponseDto>>(okResult.Value!, "data")!.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetRevenueSummary_ShouldReturnSummary()
        {
            var summary = PaymentTestData.GetRevenueSummary();

            _paymentServiceMock.Setup(x => x.GetRevenueSummaryAsync(1, null)).ReturnsAsync(summary);

            var result = await _controller.GetRevenueSummary(1, null);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Revenue summary fetched successfully");
        }

        // NEGATIVE TEST CASES

        [Fact]
        public async Task CreatePayment_ShouldReturnBadRequest_WhenBodyIsNull()
        {
            var result = await _controller.CreatePayment(null!);

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(badRequestResult!.Value!, "message").Should().Be("Request body cannot be empty");
        }

        [Fact]
        public async Task CreatePayment_ShouldThrowException_WhenServiceThrows()
        {
            var dto = PaymentTestData.GetCreatePaymentDto();

            _paymentServiceMock.Setup(x => x.CreatePaymentAsync(dto)).ThrowsAsync(new Exception("Create failed"));

            Func<Task> action = async () => await _controller.CreatePayment(dto);

            await action.Should().ThrowAsync<Exception>().WithMessage("Create failed");
        }

        [Fact]
        public async Task GetPaymentById_ShouldReturnPaymentNotFoundMessage()
        {
            _paymentServiceMock.Setup(x => x.GetPaymentByIdAsync(1)).ReturnsAsync((PaymentResponseDto?)null);

            var result = await _controller.GetPaymentById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Payment not found");
        }

        [Fact]
        public async Task GetCustomerPayments_ShouldReturnNoPaymentsMessage_WhenEmpty()
        {
            _paymentServiceMock.Setup(x => x.GetCustomerPaymentsAsync(1)).ReturnsAsync(Array.Empty<PaymentResponseDto>());

            var result = await _controller.GetCustomerPayments(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("No payments found for this customer");
        }
    }
}
