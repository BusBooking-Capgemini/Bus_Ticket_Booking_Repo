using API_Bus_Ticket_Booking.DTOs.Customer;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators
{
    public class CustomerUpdateDtoValidator : AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(255)
                .WithMessage("Name cannot exceed 255 characters.")
                .When(x => x.Name != null);

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("A valid email address is required.")
                .MaximumLength(255)
                .WithMessage("Email cannot exceed 255 characters.")
                .When(x => x.Email != null);

            RuleFor(x => x.Phone)
                .Matches(@"^\d{10}$")
                .WithMessage("Phone must be exactly 10 digits.")
                .When(x => x.Phone != null);

            RuleFor(x => x.City)
                .MaximumLength(255)
                .WithMessage("City cannot exceed 255 characters.")
                .When(x => x.City != null);

            RuleFor(x => x.State)
                .MaximumLength(255)
                .WithMessage("State cannot exceed 255 characters.")
                .When(x => x.State != null);

            RuleFor(x => x.ZipCode)
                .Matches(@"^\d{6}$")
                .WithMessage("Zip code must be exactly 6 digits.")
                .When(x => x.ZipCode != null);
        }
    }
}
