using API_Bus_Ticket_Booking.DTOs.Office;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators.Office
{
    public class OfficeCreateDtoValidator : AbstractValidator<OfficeRequestDto>
    {
        public OfficeCreateDtoValidator()
        {
            RuleFor(x => x.AgencyId)
                .GreaterThan(0)
                .WithMessage("AgencyId is required");

            RuleFor(x => x.OfficeMail)
                .NotEmpty()
                .WithMessage("Office email is required")
                .EmailAddress()
                .WithMessage("Invalid office email")
                .MaximumLength(100);

            RuleFor(x => x.OfficeContactPersonName)
                .NotEmpty()
                .WithMessage(
                    "Office contact person name is required")
                .MaximumLength(50);

            RuleFor(x => x.OfficeContactNumber)
                .NotEmpty()
                .WithMessage("Office contact number is required")
                .Matches(@"^[0-9]{10}$")
                .WithMessage(
                    "Office contact number must be 10 digits");

            RuleFor(x => x.OfficeAddressId)
                .GreaterThan(0)
                .WithMessage("OfficeAddressId is required");
        }
    }
}
