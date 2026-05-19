using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.DTOs.Bus;
using API_Bus_Ticket_Booking.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TEST_Bus_Ticket_Booking.Helpers;
using TEST_Bus_Ticket_Booking.TestData;

namespace TEST_Bus_Ticket_Booking.Controllers
{
    public class BusControllerTests
    {
        private readonly Mock<IBusService> _serviceMock;
        private readonly BusController _controller;

        public BusControllerTests()
        {
            _serviceMock = new Mock<IBusService>();
            _controller = new BusController(_serviceMock.Object);
        }

        // POSITIVE TEST CASES

        [Fact]
        public async Task GetAll_ShouldReturnAllBuses()
        {
            var buses = BusTestData.GetBusResponseDtos();

            _serviceMock.Setup(x => x.GetAllBusesAsync()).ReturnsAsync(buses);

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<bool>(okResult!.Value!, "success").Should().BeTrue();
            TestHelper.GetPropertyValue<string>(okResult.Value!, "message").Should().Be("Buses retrieved successfully.");
            TestHelper.GetPropertyValue<int>(okResult.Value!, "count").Should().Be(2);
        }

        [Fact]
        public async Task GetById_ShouldReturnBus()
        {
            var bus = BusTestData.GetBusResponseDto();

            _serviceMock.Setup(x => x.GetBusByIdAsync(1)).ReturnsAsync(bus);

            var result = await _controller.GetById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "message").Should().Be("Bus retrieved successfully.");
        }

        [Fact]
        public async Task GetByOffice_ShouldReturnBuses()
        {
            var buses = BusTestData.GetBusResponseDtos();

            _serviceMock.Setup(x => x.GetBusesByOfficeAsync(1)).ReturnsAsync(buses);

            var result = await _controller.GetByOffice(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<int>(okResult!.Value!, "count").Should().Be(2);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedBus()
        {
            var dto = BusTestData.GetBusRequestDto();
            var response = BusTestData.GetBusResponseDto();

            _serviceMock.Setup(x => x.CreateBusAsync(dto)).ReturnsAsync(response);

            var result = await _controller.Create(dto);

            var createdResult = result as CreatedAtActionResult;

            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
            TestHelper.GetPropertyValue<string>(createdResult.Value!, "message").Should().Be("Bus created successfully.");
        }

        // NEGATIVE TEST CASES

        [Fact]
        public async Task GetByType_ShouldThrowException()
        {
            _serviceMock.Setup(x => x.GetBusesByTypeAsync("AC")).ThrowsAsync(new Exception("Type failed"));

            Func<Task> action = async () => await _controller.GetByType("AC");

            await action.Should().ThrowAsync<Exception>().WithMessage("Type failed");
        }

        [Fact]
        public async Task GetByRegistration_ShouldThrowException()
        {
            _serviceMock.Setup(x => x.GetBusByRegistrationAsync("PB10AA1111")).ThrowsAsync(new Exception("Registration failed"));

            Func<Task> action = async () => await _controller.GetByRegistration("PB10AA1111");

            await action.Should().ThrowAsync<Exception>().WithMessage("Registration failed");
        }

        [Fact]
        public async Task GetByCapacityRange_ShouldThrowException()
        {
            _serviceMock.Setup(x => x.GetBusesByCapacityRangeAsync(30, 50)).ThrowsAsync(new Exception("Capacity failed"));

            Func<Task> action = async () => await _controller.GetByCapacityRange(30, 50);

            await action.Should().ThrowAsync<Exception>().WithMessage("Capacity failed");
        }

        [Fact]
        public async Task Delete_ShouldThrowException()
        {
            _serviceMock.Setup(x => x.DeleteBusAsync(1)).ThrowsAsync(new Exception("Delete failed"));

            Func<Task> action = async () => await _controller.Delete(1);

            await action.Should().ThrowAsync<Exception>().WithMessage("Delete failed");
        }

        // EDGE CASE TEST CASES

        [Fact]
        public async Task GetByCapacityRange_MinGreaterThanMax_ShouldThrowException()
        {
            _serviceMock
                .Setup(x => x.GetBusesByCapacityRangeAsync(50, 10))
                .ThrowsAsync(new Exception("Invalid capacity range"));

            Func<Task> action = async () => await _controller.GetByCapacityRange(50, 10);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid capacity range");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetById_InvalidId_ShouldThrowException(int id)
        {
            _serviceMock
                .Setup(x => x.GetBusByIdAsync(id))
                .ThrowsAsync(new Exception("Invalid bus id"));

            Func<Task> action = async () => await _controller.GetById(id);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid bus id");
        }

        [Fact]
        public async Task Create_EmptyRegistration_ShouldThrowException()
        {
            var dto = BusTestData.GetBusRequestDto();
            dto.RegistrationNumber = "";

            _serviceMock
                .Setup(x => x.CreateBusAsync(dto))
                .ThrowsAsync(new Exception("Registration number is required"));

            Func<Task> action = async () => await _controller.Create(dto);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Registration number is required");
        }

        [Fact]
        public async Task GetByOffice_InvalidOfficeId_ShouldThrowException()
        {
            _serviceMock
                .Setup(x => x.GetBusesByOfficeAsync(0))
                .ThrowsAsync(new Exception("Invalid office id"));

            Func<Task> action = async () => await _controller.GetByOffice(0);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid office id");
        }

        [Fact]
        public async Task GetByCapacityRange_BothZero_ShouldThrowException()
        {
            _serviceMock
                .Setup(x => x.GetBusesByCapacityRangeAsync(0, 0))
                .ThrowsAsync(new Exception("Capacity range cannot be zero"));

            Func<Task> action = async () => await _controller.GetByCapacityRange(0, 0);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Capacity range cannot be zero");
        }
    }
}