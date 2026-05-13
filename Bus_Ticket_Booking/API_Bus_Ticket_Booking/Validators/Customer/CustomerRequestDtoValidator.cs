using API_Bus_Ticket_Booking.DTOs.Customer;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators
{
    public class CustomerRequestDtoValidator : AbstractValidator<CustomerRequestDto>
    {
        public CustomerRequestDtoValidator()
        {
            // CREATE => all fields required
            // UPDATE => validate only provided fields

            // Name
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(255)
                .WithMessage("Name cannot exceed 255 characters.")
                .When(x => IsCreate(x));

            RuleFor(x => x.Name)
                .MaximumLength(255)
                .WithMessage("Name cannot exceed 255 characters.")
                .When(x => IsUpdate(x) && x.Name != null);

            // Email
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("A valid email address is required.")
                .MaximumLength(255)
                .WithMessage("Email cannot exceed 255 characters.")
                .When(x => IsCreate(x));

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("A valid email address is required.")
                .MaximumLength(255)
                .WithMessage("Email cannot exceed 255 characters.")
                .When(x => IsUpdate(x) && x.Email != null);

            // Phone
            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone is required.")
                .Matches(@"^\d{10}$")
                .WithMessage("Phone must be exactly 10 digits.")
                .When(x => IsCreate(x));

            RuleFor(x => x.Phone)
                .Matches(@"^\d{10}$")
                .WithMessage("Phone must be exactly 10 digits.")
                .When(x => IsUpdate(x) && x.Phone != null);

            // Address
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .When(x => IsCreate(x));

            // City
            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.")
                .MaximumLength(255)
                .WithMessage("City cannot exceed 255 characters.")
                .When(x => IsCreate(x));

            RuleFor(x => x.City)
                .MaximumLength(255)
                .WithMessage("City cannot exceed 255 characters.")
                .When(x => IsUpdate(x) && x.City != null);

            // State
            RuleFor(x => x.State)
                .NotEmpty()
                .WithMessage("State is required.")
                .MaximumLength(255)
                .WithMessage("State cannot exceed 255 characters.")
                .When(x => IsCreate(x));

            RuleFor(x => x.State)
                .MaximumLength(255)
                .WithMessage("State cannot exceed 255 characters.")
                .When(x => IsUpdate(x) && x.State != null);

            // ZipCode
            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .WithMessage("Zip code is required.")
                .Matches(@"^\d{6}$")
                .WithMessage("Zip code must be exactly 6 digits.")
                .When(x => IsCreate(x));

            RuleFor(x => x.ZipCode)
                .Matches(@"^\d{6}$")
                .WithMessage("Zip code must be exactly 6 digits.")
                .When(x => IsUpdate(x) && x.ZipCode != null);
        }

        private bool IsCreate(CustomerRequestDto dto)
        {
            return dto.Name != null
                && dto.Email != null
                && dto.Phone != null
                && dto.Address != null
                && dto.City != null
                && dto.State != null
                && dto.ZipCode != null;
        }

        private bool IsUpdate(CustomerRequestDto dto)
        {
            return !IsCreate(dto);
        }
    }
}
