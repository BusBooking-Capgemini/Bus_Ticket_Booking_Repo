using API_Bus_Ticket_Booking.DTOs.Booking;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators.Booking
{
    public class CreateBookingValidator
        : AbstractValidator<CreateBookingDto>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.TripId)
                .GreaterThan(0)
                .WithMessage("TripId must be greater than 0");

            RuleFor(x => x.SeatNumber)
                .GreaterThan(0)
                .WithMessage("SeatNumber must be greater than 0");
        }
    }
}