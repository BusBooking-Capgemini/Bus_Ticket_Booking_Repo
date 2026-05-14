using API_Bus_Ticket_Booking.Controllers;
using API_Bus_Ticket_Booking.DTOs.Office;
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

namespace TEST_Bus_Ticket_Booking.Controllers
{
    public class OfficeControllerTests
    {
        private readonly Mock<IOfficeService> _serviceMock;

        private readonly OfficeController _controller;

        public OfficeControllerTests()
        {
            _serviceMock = new Mock<IOfficeService>();

            _controller = new OfficeController(
                _serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllOffices()
        {
            var offices = new List<OfficeResponseDto>
            {
                new OfficeResponseDto
                {
                    OfficeId = 1,
                    OfficeMail = "office@test.com"
                }
            };

            _serviceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(offices);

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetById_ShouldReturnOffice_WhenOfficeExists()
        {
            var office = new OfficeResponseDto
            {
                OfficeId = 1,
                OfficeMail = "office@test.com"
            };

            _serviceMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(office);

            var result = await _controller.GetById(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();

            okResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetSummary_ShouldReturnSummary()
        {
            var summary = new
            {
                OfficeId = 1,
                TotalBuses = 5
            };

            _serviceMock
                .Setup(x => x.GetSummaryAsync(1))
                .ReturnsAsync(summary);

            var result = await _controller.GetSummary(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
        }

        [Fact]
        public async Task GetPayments_ShouldReturnPayments()
        {
            var payments = new List<object>
            {
                new
                {
                    PaymentId = 1,
                    Amount = 1200
                }
            };

            _serviceMock
                .Setup(x => x.GetPaymentsAsync(1))
                .ReturnsAsync(payments);

            var result = await _controller.GetPayments(1);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_ShouldThrowException_WhenOfficeDoesNotExist()
        {
            _serviceMock
                .Setup(x => x.GetByIdAsync(1))
                .ThrowsAsync(new NotFoundException("Office not found"));

            Func<Task> action = async () =>
                await _controller.GetById(1);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Office not found");
        }

        [Fact]
        public async Task GetSummary_ShouldThrowException_WhenSummaryDoesNotExist()
        {
            _serviceMock
                .Setup(x => x.GetSummaryAsync(1))
                .ThrowsAsync(new NotFoundException("Summary not found"));

            Func<Task> action = async () =>
                await _controller.GetSummary(1);

            await action.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetPayments_ShouldThrowException_WhenPaymentsDoNotExist()
        {
            _serviceMock
                .Setup(x => x.GetPaymentsAsync(1))
                .ThrowsAsync(new NotFoundException("Payments not found"));

            Func<Task> action = async () =>
                await _controller.GetPayments(1);

            await action.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetTrips_ShouldThrowException_WhenTripsDoNotExist()
        {
            _serviceMock
                .Setup(x => x.GetTripsAsync(1))
                .ThrowsAsync(new NotFoundException("Trips not found"));

            Func<Task> action = async () =>
                await _controller.GetTrips(1);

            await action.Should()
                .ThrowAsync<NotFoundException>();
        }
    }
}
