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
                    "Name must be at least 3 characters")

                .MaximumLength(50)
                .WithMessage(
                    "Name cannot exceed 50 characters")

                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage(
                    "Name can contain only alphabets and spaces");


            RuleFor(x => x.Email)

                .NotEmpty()
                .WithMessage("Email is required")

                .MaximumLength(100)
                .WithMessage(
                    "Email cannot exceed 100 characters")

                .EmailAddress()
                .WithMessage(
                    "Invalid email format");


            RuleFor(x => x.Phone)

                .NotEmpty()
                .WithMessage("Phone is required")

                .Matches(@"^[1-9][0-9]{9}$")
                .WithMessage(
                    "Phone number must be 10 digits and cannot start with 0");


            RuleFor(x => x.Password)

                .NotEmpty()
                .WithMessage("Password is required")

                .MinimumLength(8)
                .WithMessage(
                    "Password must be at least 8 characters")

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
                    "Password cannot contain spaces")

                .Must(p =>
                    p.Distinct().Count() > 3)
                .WithMessage(
                    "Password is too weak");
        }
    }
}