using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TEST_Bus_Ticket_Booking.Helpers;
using TEST_Bus_Ticket_Booking.TestData;

namespace TEST_Bus_Ticket_Booking.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        // POSITIVE TEST CASES

        [Fact]
        public async Task CustomerSignup_ShouldReturnSuccessResponse()
        {
            var dto = AuthTestData.GetCustomerSignupDto();
            var response = AuthTestData.GetCustomerAuthResponse();

            _authServiceMock.Setup(x => x.CustomerSignupAsync(dto)).ReturnsAsync(response);

            var result = await _controller.CustomerSignup(dto);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            TestHelper.GetPropertyValue<bool>(okResult.Value!, "success").Should().BeTrue();
            TestHelper.GetPropertyValue<string>(okResult.Value!, "message").Should().Be("Customer registered successfully");
        }

        [Fact]
        public async Task CustomerLogin_ShouldReturnSuccessResponse()
        {
            var dto = AuthTestData.GetCustomerLoginDto();
            var response = AuthTestData.GetCustomerAuthResponse();

            _authServiceMock.Setup(x => x.CustomerLoginAsync(dto)).ReturnsAsync(response);

            var result = await _controller.CustomerLogin(dto);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Customer login successful");
        }

        [Fact]
        public async Task AgencyLogin_ShouldReturnSuccessResponse()
        {
            var dto = AuthTestData.GetAgencyLoginDto();
            var response = AuthTestData.GetAgencyAuthResponse();

            _authServiceMock.Setup(x => x.AgencyLoginAsync(dto)).ReturnsAsync(response);

            var result = await _controller.AgencyLogin(dto);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Agency login successful");
        }

        [Fact]
        public async Task OfficeLogin_ShouldReturnSuccessResponse()
        {
            var dto = AuthTestData.GetOfficeLoginDto();
            var response = AuthTestData.GetOfficeAuthResponse();

            _authServiceMock.Setup(x => x.OfficeLoginAsync(dto)).ReturnsAsync(response);

            var result = await _controller.OfficeLogin(dto);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Office login successful");
        }

        // NEGATIVE TEST CASES

        [Fact]
        public async Task CustomerSignup_ShouldThrowException()
        {
            var dto = AuthTestData.GetCustomerSignupDto();

            _authServiceMock.Setup(x => x.CustomerSignupAsync(dto)).ThrowsAsync(new Exception("Signup failed"));

            Func<Task> action = async () => await _controller.CustomerSignup(dto);

            await action.Should().ThrowAsync<Exception>().WithMessage("Signup failed");
        }

        [Fact]
        public async Task CustomerLogin_ShouldThrowException()
        {
            var dto = AuthTestData.GetCustomerLoginDto();

            _authServiceMock.Setup(x => x.CustomerLoginAsync(dto)).ThrowsAsync(new Exception("Login failed"));

            Func<Task> action = async () => await _controller.CustomerLogin(dto);

            await action.Should().ThrowAsync<Exception>().WithMessage("Login failed");
        }

        [Fact]
        public async Task AgencyLogin_ShouldThrowException()
        {
            var dto = AuthTestData.GetAgencyLoginDto();

            _authServiceMock.Setup(x => x.AgencyLoginAsync(dto)).ThrowsAsync(new Exception("Login failed"));

            Func<Task> action = async () => await _controller.AgencyLogin(dto);

            await action.Should().ThrowAsync<Exception>().WithMessage("Login failed");
        }

        [Fact]
        public async Task OfficeLogin_ShouldThrowException()
        {
            var dto = AuthTestData.GetOfficeLoginDto();

            _authServiceMock.Setup(x => x.OfficeLoginAsync(dto)).ThrowsAsync(new Exception("Login failed"));

            Func<Task> action = async () => await _controller.OfficeLogin(dto);

            await action.Should().ThrowAsync<Exception>().WithMessage("Login failed");
        }
    }
}