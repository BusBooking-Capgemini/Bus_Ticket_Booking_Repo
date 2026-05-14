using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.DTOs.Trip;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TEST_Bus_Ticket_Booking.Helpers;
using TEST_Bus_Ticket_Booking.TestData;

namespace TEST_Bus_Ticket_Booking.Controllers
{
    public class TripControllerTests
    {
        private readonly Mock<ITripService> _tripServiceMock;
        private readonly TripController _controller;

        public TripControllerTests()
        {
            _tripServiceMock = new Mock<ITripService>();
            _controller = new TripController(_tripServiceMock.Object);
        }

        // POSITIVE TEST CASES

        [Fact]
        public async Task GetAll_ShouldReturnTrips()
        {
            var trips = TripTestData.GetTripResponseDtos();

            _tripServiceMock.Setup(x => x.GetAllTripsAsync()).ReturnsAsync(trips);

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<bool>(okResult!.Value!, "Success").Should().BeTrue();
            TestHelper.GetPropertyValue<string>(okResult.Value!, "Message").Should().Be("2 trip(s) retrieved successfully.");
        }

        [Fact]
        public async Task GetById_ShouldReturnTrip()
        {
            var trip = TripTestData.GetTripResponseDto();

            _tripServiceMock.Setup(x => x.GetTripByIdAsync(1)).ReturnsAsync(trip);

            var result = await _controller.GetById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "Message").Should().Be("Trip retrieved successfully.");
        }

        [Fact]
        public async Task Search_ShouldReturnTrips()
        {
            var dto = TripTestData.GetTripSearchDto();
            var trips = TripTestData.GetTripResponseDtos();

            _tripServiceMock.Setup(x => x.SearchTripsAsync(dto)).ReturnsAsync(trips);

            var result = await _controller.Search(dto);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "Message").Should().Contain("trip(s) found from");
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedTrip()
        {
            var dto = TripTestData.GetCreateTripDto();
            var trip = TripTestData.GetTripResponseDto();

            _tripServiceMock.Setup(x => x.CreateTripAsync(dto)).ReturnsAsync(trip);

            var result = await _controller.Create(dto);

            var createdResult = result as CreatedAtActionResult;

            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
        }

        // NEGATIVE TEST CASES

        [Fact]
        public async Task Search_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("FromCity", "Required");

            var result = await _controller.Search(new TripSearchDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetByRoute_ShouldReturnNotFound_WhenRouteDoesNotExist()
        {
            _tripServiceMock.Setup(x => x.GetTripsByRouteAsync(1)).ThrowsAsync(new NotFoundException("Route not found"));

            var result = await _controller.GetByRoute(1);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("BusId", "Required");

            var result = await _controller.Update(1, new UpdateTripDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenTripDoesNotExist()
        {
            _tripServiceMock.Setup(x => x.DeleteTripAsync(1)).ThrowsAsync(new NotFoundException("Trip not found"));

            var result = await _controller.Delete(1);

            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}