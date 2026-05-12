using API_Bus_Ticket_Booking.DTOs.Auth;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators.Auth
{
    public class CustomerSignupValidator
        : AbstractValidator<CustomerSignupDto>
    {
        public CustomerSignupValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")

                .MinimumLength(3)
                .WithMessage(
                    "Name must be at least 3 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")

                .EmailAddress()
                .WithMessage("Invalid email format");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone is required")

                .Matches(@"^[0-9]{10}$")
                .WithMessage(
                    "Phone number must be 10 digits");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")

                .MinimumLength(6)
                .WithMessage(
                    "Password must be at least 6 characters")

                .Matches("[A-Z]")
                .WithMessage(
                    "Password must contain at least one uppercase letter")

                .Matches("[a-z]")
                .WithMessage(
                    "Password must contain at least one lowercase letter")

                .Matches("[0-9]")
                .WithMessage(
                    "Password must contain at least one number");
        }
    }
}