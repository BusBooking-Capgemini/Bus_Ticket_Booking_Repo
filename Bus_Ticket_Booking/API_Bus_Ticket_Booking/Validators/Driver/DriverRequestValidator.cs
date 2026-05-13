using API_Bus_Ticket_Booking.DTOs.Driver;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators
{
    public class DriverRequestValidator : AbstractValidator<DriverRequestDto>
    {
        public DriverRequestValidator()
        {
            RuleFor(x => x.LicenseNumber)
                .NotEmpty().WithMessage("License number is required.")
                .MaximumLength(20).WithMessage("License number cannot exceed 20 characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Driver name is required.")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone must be exactly 10 digits.");

            RuleFor(x => x.OfficeId)
                .GreaterThan(0).WithMessage("OfficeId must be a positive number.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(255).WithMessage("City cannot exceed 255 characters.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(255).WithMessage("State cannot exceed 255 characters.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip code is required.")
                .Matches(@"^\d{6}$").WithMessage("Zip code must be exactly 6 digits.");
        }
    }
}