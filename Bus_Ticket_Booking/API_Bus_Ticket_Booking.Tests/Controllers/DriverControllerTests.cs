using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TEST_Bus_Ticket_Booking.Helpers;
using TEST_Bus_Ticket_Booking.TestData;

namespace TEST_Bus_Ticket_Booking.Controllers
{
    public class DriverControllerTests
    {
        private readonly Mock<IDriverService> _serviceMock;
        private readonly DriverController _controller;

        public DriverControllerTests()
        {
            _serviceMock = new Mock<IDriverService>();
            _controller = new DriverController(_serviceMock.Object);
        }

        // POSITIVE TEST CASES

        [Fact]
        public async Task GetAll_ShouldReturnAllDrivers()
        {
            var drivers = DriverTestData.GetDriverResponseDtos();

            _serviceMock.Setup(x => x.GetAllDriversAsync()).ReturnsAsync(drivers);

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<bool>(okResult!.Value!, "success").Should().BeTrue();
            TestHelper.GetPropertyValue<int>(okResult.Value!, "count").Should().Be(2);
        }

        [Fact]
        public async Task GetById_ShouldReturnDriver()
        {
            var driver = DriverTestData.GetDriverResponseDto();

            _serviceMock.Setup(x => x.GetDriverByIdAsync(1)).ReturnsAsync(driver);

            var result = await _controller.GetById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Driver retrieved successfully.");
        }

        [Fact]
        public async Task GetByOffice_ShouldReturnDrivers()
        {
            var drivers = DriverTestData.GetDriverResponseDtos();

            _serviceMock.Setup(x => x.GetDriversByOfficeAsync(1)).ReturnsAsync(drivers);

            var result = await _controller.GetByOffice(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<int>(okResult!.Value!, "count").Should().Be(2);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedDriver()
        {
            var dto = DriverTestData.GetDriverRequestDto();
            var driver = DriverTestData.GetDriverResponseDto();

            _serviceMock.Setup(x => x.CreateDriverAsync(dto)).ReturnsAsync(driver);

            var result = await _controller.Create(dto);

            var createdResult = result as CreatedAtActionResult;

            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
        }

        // NEGATIVE TEST CASES

        [Fact]
        public async Task GetByLicense_ShouldThrowException()
        {
            _serviceMock.Setup(x => x.GetDriverByLicenseAsync("LIC12345")).ThrowsAsync(new Exception("License failed"));

            Func<Task> action = async () => await _controller.GetByLicense("LIC12345");

            await action.Should().ThrowAsync<Exception>().WithMessage("License failed");
        }

        [Fact]
        public async Task SearchByName_ShouldThrowException()
        {
            _serviceMock.Setup(x => x.GetDriversByNameAsync("Rohit")).ThrowsAsync(new Exception("Search failed"));

            Func<Task> action = async () => await _controller.SearchByName("Rohit");

            await action.Should().ThrowAsync<Exception>().WithMessage("Search failed");
        }

        [Fact]
        public async Task GetByCity_ShouldThrowException()
        {
            _serviceMock.Setup(x => x.GetDriversByCityAsync("Delhi")).ThrowsAsync(new Exception("City failed"));

            Func<Task> action = async () => await _controller.GetByCity("Delhi");

            await action.Should().ThrowAsync<Exception>().WithMessage("City failed");
        }

        [Fact]
        public async Task Delete_ShouldThrowException()
        {
            _serviceMock.Setup(x => x.DeleteDriverAsync(1)).ThrowsAsync(new Exception("Delete failed"));

            Func<Task> action = async () => await _controller.Delete(1);

            await action.Should().ThrowAsync<Exception>().WithMessage("Delete failed");
        }
    }
}