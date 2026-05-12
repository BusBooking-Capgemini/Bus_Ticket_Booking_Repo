using API_Bus_Ticket_Booking.DTOs.Route;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators.Route
{
    public class UpdateRouteDtoValidator : AbstractValidator<UpdateRouteDto>
    {
        public UpdateRouteDtoValidator()
        {
            RuleFor(x => x.FromCity)
                .MaximumLength(255).WithMessage("FromCity cannot exceed 255 characters.")
                .When(x => x.FromCity != null);

            RuleFor(x => x.ToCity)
                .MaximumLength(255).WithMessage("ToCity cannot exceed 255 characters.")
                .When(x => x.ToCity != null);

            RuleFor(x => x.BreakPoints)
                .GreaterThanOrEqualTo(0).WithMessage("BreakPoints cannot be negative.")
                .When(x => x.BreakPoints.HasValue);

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than zero.")
                .When(x => x.Duration.HasValue);
        }
    }
}
