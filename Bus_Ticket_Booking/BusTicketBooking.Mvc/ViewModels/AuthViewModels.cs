using System.ComponentModel.DataAnnotations;

namespace BusTicketBooking.Mvc.ViewModels;

public sealed class LoginViewModel
{
    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    public string Role { get; set; } = "Customer";
}

public sealed class CustomerSignupViewModel
{
    [Required, StringLength(100)]
    public string? Name { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? Phone { get; set; }

    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }
}

public sealed class DashboardViewModel
{
    public string Role { get; set; } = "Guest";
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public int? AgencyId { get; set; }
    public int? OfficeId { get; set; }
    public int? CustomerId { get; set; }
    public IReadOnlyList<DashboardActionViewModel> Actions { get; set; } = Array.Empty<DashboardActionViewModel>();
}

public sealed class DashboardActionViewModel
{
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required string Controller { get; init; }
    public required string Action { get; init; }
    public IDictionary<string, string>? RouteValues { get; init; }
}
