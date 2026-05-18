using API_Bus_Ticket_Booking.DTOs.Auth;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators.Auth
{
    public class LoginRequestValidator
        : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)

                .NotEmpty()
                .WithMessage("Email is required")

                .MaximumLength(100)
                .WithMessage(
                    "Email cannot exceed 100 characters")

                .EmailAddress()
                .WithMessage(
                    "Invalid email format");


            RuleFor(x => x.Password)

                .NotEmpty()
                .WithMessage("Password is required")

                .MinimumLength(6)
                .WithMessage(
                    "Password must be at least 6 characters")

                .MaximumLength(50)
                .WithMessage(
                    "Password cannot exceed 50 characters")

                .Matches("[A-Z]")
                .WithMessage(
                    "Password must contain at least one uppercase letter")

                .Matches("[a-z]")
                .WithMessage(
                    "Password must contain at least one lowercase letter")

                .Matches("[0-9]")
                .WithMessage(
                    "Password must contain at least one number")

                .Matches("[^a-zA-Z0-9]")
                .WithMessage(
                    "Password must contain at least one special character")

                .Must(p => !p.Contains(" "))
                .WithMessage(
                    "Password cannot contain spaces");
        }
    }
}