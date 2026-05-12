using API_Bus_Ticket_Booking.DTOs.Trip;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators.Trip
{
    public class UpdateTripDtoValidator : AbstractValidator<UpdateTripDto>
    {
        public UpdateTripDtoValidator()
        {
            RuleFor(x => x.BusId)
                .GreaterThan(0).WithMessage("BusId must be a valid ID.")
                .When(x => x.BusId.HasValue);

            RuleFor(x => x.Driver1DriverId)
                .GreaterThan(0).WithMessage("Driver1DriverId must be a valid ID.")
                .When(x => x.Driver1DriverId.HasValue);

            RuleFor(x => x.Driver2DriverId)
                .GreaterThan(0).WithMessage("Driver2DriverId must be a valid ID.")
                .When(x => x.Driver2DriverId.HasValue);

            RuleFor(x => x.Fare)
                .GreaterThan(0).WithMessage("Fare must be greater than zero.")
                .When(x => x.Fare.HasValue);
        }
    }
}
