using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEST_Bus_Ticket_Booking.TestData;

namespace TEST_Bus_Ticket_Booking.Controllers
{
    public class AgencyControllerTests
    {
        private readonly Mock<IAgencyService> _serviceMock;

        private readonly AgencyController _controller;

        public AgencyControllerTests()
        {
            _serviceMock = new Mock<IAgencyService>();

            _controller = new AgencyController(_serviceMock.Object);
        }

        // POSITIVE TEST CASES

        [Fact]
        public async Task GetAll_ShouldReturnAllAgencies()
        {
            // Arrange
            var agencies = AgencyTestData.GetAgencies();

            _serviceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(agencies);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult!.StatusCode.Should().Be(200);

            var response = okResult.Value;

            response.Should().NotBeNull();

            response!.GetType().GetProperty("success")!
                .GetValue(response)!
                .Should().Be(true);

            response.GetType().GetProperty("message")!
                .GetValue(response)!
                .Should().Be("Agencies retrieved successfully");

            response.GetType().GetProperty("count")!
                .GetValue(response)!
                .Should().Be(2);
        }

        [Fact]
        public async Task GetAgencySummary_ShouldReturnSummary()
        {
            // A
            var summary = AgencyTestData.GetAgencySummary();

            _serviceMock
                .Setup(x => x.GetAgencySummaryAsync(1))
                .ReturnsAsync(summary);

            // A
            var result = await _controller.GetAgencySummary(1);

            // A
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetOfficeBookings_ShouldReturnBookings()
        {
            // A
            var bookings = AgencyTestData.GetOfficeBookings();

            _serviceMock
                .Setup(x => x.GetOfficeBookingsAsync(1, 1))
                .ReturnsAsync(bookings);

            // A
            var result = await _controller.GetOfficeBookings(1, 1);

            // A
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetOfficePayments_ShouldReturnPayments()
        {
            // A
            var payments = AgencyTestData.GetOfficePayments();

            _serviceMock
                .Setup(x => x.GetOfficePaymentsAsync(1, 1))
                .ReturnsAsync(payments);

            // A
            var result = await _controller.GetOfficePayments(1, 1);

            // A
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult!.StatusCode.Should().Be(200);
        }

        // NEGATIVE TEST CASES

        [Fact]
        public async Task GetAgencySummary_ShouldThrowNotFoundException()
        {
            // A
            _serviceMock
                .Setup(x => x.GetAgencySummaryAsync(99))
                .ThrowsAsync(new NotFoundException("Agency not found"));

            // A
            Func<Task> action = async () => await _controller.GetAgencySummary(99);

            // A
            await action.Should().ThrowAsync<NotFoundException>().WithMessage("Agency not found");
        }

        [Fact]
        public async Task GetOfficeBookings_ShouldThrowNotFoundException()
        {
            // A
            _serviceMock
                .Setup(x => x.GetOfficeBookingsAsync(99, 99))
                .ThrowsAsync(new NotFoundException("Bookings not found"));

            // A
            Func<Task> action = async () => await _controller.GetOfficeBookings(99, 99);

            // A
            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetOfficePayments_ShouldThrowNotFoundException()
        {
            // A
            _serviceMock
                .Setup(x => x.GetOfficePaymentsAsync(99, 99))
                .ThrowsAsync(new NotFoundException("Payments not found"));

            // A
            Func<Task> action = async () => await _controller.GetOfficePayments(99, 99);

            // A
            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetAll_ShouldThrowException()
        {
            // A
            _serviceMock
                .Setup(x => x.GetAllAsync())
                .ThrowsAsync(new Exception("Something went wrong"));

            // A
            Func<Task> action = async () => await _controller.GetAll();

            // A
            await action.Should().ThrowAsync<Exception>();
        }
    }
}
