using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.DTOs.Route;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TEST_Bus_Ticket_Booking.Helpers;
using TEST_Bus_Ticket_Booking.TestData;

namespace TEST_Bus_Ticket_Booking.Controllers
{
    public class RouteControllerTests
    {
        private readonly Mock<IRouteService> _routeServiceMock;
        private readonly RouteController _controller;

        public RouteControllerTests()
        {
            _routeServiceMock = new Mock<IRouteService>();
            _controller = new RouteController(_routeServiceMock.Object);
        }

        // POSITIVE TEST CASES

        [Fact]
        public async Task GetAll_ShouldReturnRoutes()
        {
            var routes = RouteTestData.GetRouteResponseDtos();

            _routeServiceMock.Setup(x => x.GetAllRoutesAsync()).ReturnsAsync(routes);

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<bool>(okResult!.Value!, "Success").Should().BeTrue();
            TestHelper.GetPropertyValue<string>(okResult.Value!, "Message").Should().Be("2 route(s) retrieved successfully.");
        }

        [Fact]
        public async Task GetById_ShouldReturnRoute()
        {
            var route = RouteTestData.GetRouteResponseDto();

            _routeServiceMock.Setup(x => x.GetRouteByIdAsync(1)).ReturnsAsync(route);

            var result = await _controller.GetById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "Message").Should().Be("Route retrieved successfully.");
        }

        [Fact]
        public async Task Search_ShouldReturnRoutes()
        {
            var routes = RouteTestData.GetRouteResponseDtos();

            _routeServiceMock.Setup(x => x.SearchRoutesAsync("Delhi", "Mumbai")).ReturnsAsync(routes);

            var result = await _controller.Search("Delhi", "Mumbai");

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<string>(okResult!.Value!, "Message").Should().Contain("route(s) found matching your search");
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedRoute()
        {
            var dto = RouteTestData.GetCreateRouteDto();
            var route = RouteTestData.GetRouteResponseDto();

            _routeServiceMock.Setup(x => x.CreateRouteAsync(dto)).ReturnsAsync(route);

            var result = await _controller.Create(dto);

            var createdResult = result as CreatedAtActionResult;

            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
        }

        // NEGATIVE TEST CASES

        [Fact]
        public async Task Search_ShouldReturnBadRequest_WhenCitiesAreMissing()
        {
            var result = await _controller.Search(string.Empty, "Mumbai");

            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
            TestHelper.GetPropertyValue<bool>(badRequestResult!.Value!, "Success").Should().BeFalse();
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRouteDoesNotExist()
        {
            _routeServiceMock.Setup(x => x.GetRouteByIdAsync(1)).ThrowsAsync(new NotFoundException("Route not found"));

            var result = await _controller.GetById(1);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenRouteDoesNotExist()
        {
            var dto = RouteTestData.GetUpdateRouteDto();

            _routeServiceMock.Setup(x => x.UpdateRouteAsync(1, dto)).ThrowsAsync(new NotFoundException("Route not found"));

            var result = await _controller.Update(1, dto);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRouteDoesNotExist()
        {
            _routeServiceMock.Setup(x => x.DeleteRouteAsync(1)).ThrowsAsync(new NotFoundException("Route not found"));

            var result = await _controller.Delete(1);

            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}