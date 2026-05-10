using API_Bus_Ticket_Booking.DTOs.Bus;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators
{
    public class BusRequestValidator : AbstractValidator<BusRequestDto>
    {
        private static readonly string[] AllowedTypes =
            { "Seater", "AC Seater", "Sleeper", "AC Sleeper", "Semi-Sleeper" };

        public BusRequestValidator()
        {
            RuleFor(x => x.OfficeId)
                .GreaterThan(0).WithMessage("OfficeId must be a positive number.");

            RuleFor(x => x.RegistrationNumber)
                .NotEmpty().WithMessage("Registration number is required.")
                .MaximumLength(20).WithMessage("Registration number cannot exceed 20 characters.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Capacity cannot exceed 100.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Bus type is required.")
                .Must(t => AllowedTypes.Contains(t))
                .WithMessage($"Type must be one of: {string.Join(", ", AllowedTypes)}");
        }
    }
}