using API_Bus_Ticket_Booking.DTOs.Review;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators
{
    public class ReviewRequestDtoValidator : AbstractValidator<ReviewRequestDto>
    {
        public ReviewRequestDtoValidator()
        {
            // CREATE: TripId is required
            RuleFor(x => x.TripId)
                .NotNull()
                .WithMessage("TripId is required.")
                .GreaterThan(0)
                .WithMessage("TripId must be a positive number.")
                .When(x => !x.ReviewId.HasValue);

            // UPDATE: ReviewId is required
            RuleFor(x => x.ReviewId)
                .NotNull()
                .WithMessage("ReviewId is required.")
                .GreaterThan(0)
                .WithMessage("ReviewId must be a positive number.")
                .When(x => x.ReviewId.HasValue);

            // Rating validation
            RuleFor(x => x.Rating)
                .NotNull()
                .WithMessage("Rating is required.")
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.")
                .When(x => !x.ReviewId.HasValue); // create

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.")
                .When(x => x.ReviewId.HasValue && x.Rating.HasValue); // update

            // Comment validation
            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .WithMessage("Comment cannot exceed 1000 characters.")
                .When(x => x.Comment != null);
        }
    }
}
