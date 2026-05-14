using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.DTOs.Customer;
using API_Bus_Ticket_Booking.DTOs.Review;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TEST_Bus_Ticket_Booking.Helpers;
using TEST_Bus_Ticket_Booking.TestData;

namespace TEST_Bus_Ticket_Booking.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly Mock<IReviewService> _reviewServiceMock;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _reviewServiceMock = new Mock<IReviewService>();
            _controller = new CustomerController(_customerServiceMock.Object, _reviewServiceMock.Object);
        }

        // POSITIVE TEST CASES

        private void SetCustomerUser(int userId)
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = TestHelper.CreatePrincipal(userId, "Customer") }
            };
        }

        [Fact]
        public async Task Register_ShouldReturnCreatedResult()
        {
            var dto = CustomerTestData.GetCustomerRequestDto();
            var customer = CustomerTestData.GetCustomerResponseDto();

            _customerServiceMock.Setup(x => x.EmailAlreadyExistsAsync(dto.Email!)).ReturnsAsync(false);
            _customerServiceMock.Setup(x => x.CreateCustomerAsync(dto)).ReturnsAsync(customer);

            var result = await _controller.Register(dto);

            var createdResult = result as CreatedAtActionResult;

            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task GetProfile_ShouldReturnCustomer()
        {
            SetCustomerUser(1);
            var customer = CustomerTestData.GetCustomerResponseDto();

            _customerServiceMock.Setup(x => x.GetCustomerAsync(1)).ReturnsAsync(customer);

            var result = await _controller.GetProfile();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<int>(okResult!.Value!, "CustomerId").Should().Be(1);
        }

        [Fact]
        public async Task UpdateProfile_ShouldReturnNoContent()
        {
            SetCustomerUser(1);
            var dto = CustomerTestData.GetCustomerRequestDto();

            _customerServiceMock.Setup(x => x.UpdateCustomerAsync(1, dto)).ReturnsAsync(true);

            var result = await _controller.UpdateProfile(dto);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetMyReviews_ShouldReturnReviews()
        {
            SetCustomerUser(1);
            var reviews = CustomerTestData.GetReviewResponses();

            _reviewServiceMock.Setup(x => x.GetCustomerReviewsAsync(1)).ReturnsAsync(reviews);

            var result = await _controller.GetMyReviews(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            var reviewList = okResult!.Value as IEnumerable<ReviewResponseDto>;

            reviewList.Should().NotBeNull();
            reviewList!.Should().HaveCount(2);
        }

        // NEGATIVE TEST CASES

        [Fact]
        public async Task Register_ShouldReturnConflict_WhenEmailExists()
        {
            var dto = CustomerTestData.GetCustomerRequestDto();

            _customerServiceMock.Setup(x => x.EmailAlreadyExistsAsync(dto.Email!)).ReturnsAsync(true);

            var result = await _controller.Register(dto);

            result.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task GetProfile_ShouldReturnNotFound_WhenCustomerMissing()
        {
            SetCustomerUser(1);

            _customerServiceMock.Setup(x => x.GetCustomerAsync(1)).ReturnsAsync((CustomerResponseDto?)null);

            var result = await _controller.GetProfile();

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateProfile_ShouldReturnNotFound_WhenUpdateFails()
        {
            SetCustomerUser(1);
            var dto = CustomerTestData.GetCustomerRequestDto();

            _customerServiceMock.Setup(x => x.UpdateCustomerAsync(1, dto)).ReturnsAsync(false);

            var result = await _controller.UpdateProfile(dto);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetMyReviews_ShouldThrowForbiddenException_WhenUserDoesNotMatch()
        {
            SetCustomerUser(2);

            Func<Task> action = async () => await _controller.GetMyReviews(1);

            await action.Should().ThrowAsync<ForbiddenException>().WithMessage("You can only access your own reviews.");
        }
    }
}