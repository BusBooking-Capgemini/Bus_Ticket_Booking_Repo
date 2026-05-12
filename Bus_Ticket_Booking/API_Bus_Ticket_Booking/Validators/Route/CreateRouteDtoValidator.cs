using API_Bus_Ticket_Booking.DTOs.Route;
using FluentValidation;

namespace API_Bus_Ticket_Booking.Validators.Route
{
    public class CreateRouteDtoValidator : AbstractValidator<CreateRouteDto>
    {
        public CreateRouteDtoValidator()
        {
            RuleFor(x => x.FromCity)
                .NotEmpty().WithMessage("FromCity is required.")
                .MaximumLength(255).WithMessage("FromCity cannot exceed 255 characters.");

            RuleFor(x => x.ToCity)
                .NotEmpty().WithMessage("ToCity is required.")
                .MaximumLength(255).WithMessage("ToCity cannot exceed 255 characters.");

            RuleFor(x => x)
                .Must(x => x.FromCity != x.ToCity)
                .WithMessage("FromCity and ToCity cannot be the same.")
                .When(x => x.FromCity != null && x.ToCity != null);

            RuleFor(x => x.BreakPoints)
                .GreaterThanOrEqualTo(0).WithMessage("BreakPoints cannot be negative.")
                .When(x => x.BreakPoints.HasValue);

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than zero.")
                .When(x => x.Duration.HasValue);
        }
    }
}
