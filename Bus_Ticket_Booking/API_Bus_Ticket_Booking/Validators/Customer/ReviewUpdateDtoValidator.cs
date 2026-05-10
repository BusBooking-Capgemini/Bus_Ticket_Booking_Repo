using API_Bus_Ticket_Booking.DTOs.Review;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators
{
    public class ReviewUpdateDtoValidator : AbstractValidator<ReviewUpdateDto>
    {
        public ReviewUpdateDtoValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.")
                .When(x => x.Rating.HasValue);

            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .WithMessage("Comment cannot exceed 1000 characters.")
                .When(x => x.Comment != null);
        }
    }
}
