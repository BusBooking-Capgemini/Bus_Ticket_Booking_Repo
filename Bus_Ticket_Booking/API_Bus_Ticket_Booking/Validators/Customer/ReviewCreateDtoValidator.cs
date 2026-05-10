using API_Bus_Ticket_Booking.DTOs.Review;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators
{
    public class ReviewCreateDtoValidator : AbstractValidator<ReviewCreateDto>
    {
        public ReviewCreateDtoValidator()
        {
            RuleFor(x => x.TripId).GreaterThan(0).WithMessage("TripId must be a positive number.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .WithMessage("Comment cannot exceed 1000 characters.")
                .When(x => x.Comment != null);
        }
    }
}
