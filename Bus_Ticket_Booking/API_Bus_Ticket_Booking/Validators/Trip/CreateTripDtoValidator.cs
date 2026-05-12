using API_Bus_Ticket_Booking.DTOs.Trip;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators.Trip
{
    public class CreateTripDtoValidator : AbstractValidator<CreateTripDto>
    {
        public CreateTripDtoValidator()
        {
            RuleFor(x => x.RouteId)
                .GreaterThan(0).WithMessage("RouteId must be a valid ID.");

            RuleFor(x => x.BusId)
                .GreaterThan(0).WithMessage("BusId must be a valid ID.");

            RuleFor(x => x.BoardingAddressId)
                .GreaterThan(0).WithMessage("BoardingAddressId must be a valid ID.");

            RuleFor(x => x.DroppingAddressId)
                .GreaterThan(0).WithMessage("DroppingAddressId must be a valid ID.");

            RuleFor(x => x.Driver1DriverId)
                .GreaterThan(0).WithMessage("Driver1DriverId must be a valid ID.");

            RuleFor(x => x.Driver2DriverId)
                .GreaterThan(0).WithMessage("Driver2DriverId must be a valid ID.");

            RuleFor(x => x.Fare)
                .GreaterThan(0).WithMessage("Fare must be greater than zero.");

            RuleFor(x => x.TripDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("TripDate cannot be in the past.");

            RuleFor(x => x.ArrivalTime)
                .GreaterThan(x => x.DepartureTime)
                .WithMessage("ArrivalTime must be after DepartureTime.");
        }
    }
}
