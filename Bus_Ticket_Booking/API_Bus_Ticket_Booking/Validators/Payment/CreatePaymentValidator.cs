using API_Bus_Ticket_Booking.DTOs.Payment;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators.Payment
{
    public class CreatePaymentValidator
        : AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentValidator()
        {
            RuleFor(x => x.BookingId)
                .GreaterThan(0)
                .WithMessage("BookingId must be greater than 0");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("CustomerId must be greater than 0");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0");
        }
    }
}