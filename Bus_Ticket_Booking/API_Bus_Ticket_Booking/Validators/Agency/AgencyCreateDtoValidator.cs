using API_Bus_Ticket_Booking.DTOs.Agency;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators.Agency
{
    public class AgencyCreateDtoValidator : AbstractValidator<AgencyRequestDto>
    {
        public AgencyCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Agency name is required")
                .MaximumLength(255);

            RuleFor(x => x.ContactPersonName)
                .NotEmpty()
                .WithMessage("Contact person name is required")
                .MaximumLength(30);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email format")
                .MaximumLength(255);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone number is required")
                .Matches(@"^[0-9]{10,15}$")
                .WithMessage(
                    "Phone number must contain only digits");
        }
    }
}
