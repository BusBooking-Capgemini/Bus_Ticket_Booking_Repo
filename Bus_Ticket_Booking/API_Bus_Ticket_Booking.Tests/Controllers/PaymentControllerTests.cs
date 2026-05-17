using System.Security.Claims;
using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.DTOs.Payment;
using API_Bus_Ticket_Booking.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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

            _paymentServiceMock
                .Setup(x => x.CreatePaymentAsync(dto))
                .ReturnsAsync(response);

            var result = await _controller.CreatePayment(dto);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            TestHelper
                .GetPropertyValue<string>(okResult.Value!, "message")
                .Should()
                .Be("Payment created successfully");
        }

        [Fact]
        public async Task GetPaymentById_ShouldReturnPayment()
        {
            var response = PaymentTestData.GetPaymentResponse();

            _paymentServiceMock
                .Setup(x => x.GetPaymentByIdAsync(1))
                .ReturnsAsync(response);

            var result = await _controller.GetPaymentById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            TestHelper
                .GetPropertyValue<string>(okResult!.Value!, "message")
                .Should()
                .Be("Payment fetched successfully");
        }

        [Fact]
        public async Task GetMyPayments_ShouldReturnPayments()
        {
            var payments = PaymentTestData.GetPayments();

            _paymentServiceMock
                .Setup(x => x.GetCustomerPaymentsAsync(1))
                .ReturnsAsync(payments);

            var claims = new List<Claim>
            {
                new Claim("CustomerId", "1")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            var result = await _controller.GetMyPayments();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            TestHelper
                .GetPropertyValue<string>(okResult!.Value!, "message")
                .Should()
                .Be("Payments fetched successfully");

            TestHelper
                .GetPropertyValue<IEnumerable<PaymentResponseDto>>(okResult.Value!, "data")!
                .Should()
                .HaveCount(2);
        }

        [Fact]
        public async Task GetRevenueSummary_ShouldReturnSummary()
        {
            var summary = PaymentTestData.GetRevenueSummary();

            _paymentServiceMock
                .Setup(x => x.GetRevenueSummaryAsync(1, null))
                .ReturnsAsync(summary);

            var result = await _controller.GetRevenueSummary(1, null);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            TestHelper
                .GetPropertyValue<string>(okResult!.Value!, "message")
                .Should()
                .Be("Revenue summary fetched successfully");
        }

        [Fact]
        public async Task GetDashboard_ShouldReturnDashboard()
        {
            var dashboard = PaymentTestData.GetDashboard();

            _paymentServiceMock
                .Setup(x => x.GetDashboardAsync(1, null))
                .ReturnsAsync(dashboard);

            var result = await _controller.GetDashboard(1, null);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            TestHelper
                .GetPropertyValue<string>(okResult!.Value!, "message")
                .Should()
                .Be("Payment dashboard fetched successfully");
        }

        [Fact]
        public async Task GetAnalytics_ShouldReturnAnalytics()
        {
            var analytics = PaymentTestData.GetAnalytics();

            _paymentServiceMock
                .Setup(x => x.GetAnalyticsAsync(1, null))
                .ReturnsAsync(analytics);

            var result = await _controller.GetAnalytics(1, null);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            TestHelper
                .GetPropertyValue<string>(okResult!.Value!, "message")
                .Should()
                .Be("Payment analytics fetched successfully");
        }

        // NEGATIVE TEST CASES

        [Fact]
        public async Task CreatePayment_ShouldReturnBadRequest_WhenBodyIsNull()
        {
            var result = await _controller.CreatePayment(null!);

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();

            TestHelper
                .GetPropertyValue<string>(badRequestResult!.Value!, "message")
                .Should()
                .Be("Request body cannot be empty");
        }

        [Fact]
        public async Task CreatePayment_ShouldThrowException_WhenServiceThrows()
        {
            var dto = PaymentTestData.GetCreatePaymentDto();

            _paymentServiceMock
                .Setup(x => x.CreatePaymentAsync(dto))
                .ThrowsAsync(new Exception("Create failed"));

            Func<Task> action = async () =>
                await _controller.CreatePayment(dto);

            await action
                .Should()
                .ThrowAsync<Exception>()
                .WithMessage("Create failed");
        }

        [Fact]
        public async Task GetPaymentById_ShouldReturnPaymentNotFoundMessage()
        {
            _paymentServiceMock
                .Setup(x => x.GetPaymentByIdAsync(1))
                .ReturnsAsync((PaymentResponseDto?)null);

            var result = await _controller.GetPaymentById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            TestHelper
                .GetPropertyValue<string>(okResult!.Value!, "message")
                .Should()
                .Be("Payment not found");
        }

        [Fact]
        public async Task GetMyPayments_ShouldReturnUnauthorized_WhenCustomerIdMissing()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await _controller.GetMyPayments();

            var unauthorizedResult = result as UnauthorizedObjectResult;

            unauthorizedResult.Should().NotBeNull();

            TestHelper
                .GetPropertyValue<string>(unauthorizedResult!.Value!, "message")
                .Should()
                .Be("Customer ID not found");
        }
    }
}