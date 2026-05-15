using BusTicketBooking.Mvc.Models;

namespace BusTicketBooking.Mvc.ViewModels;

public sealed class TripListViewModel
{
    public required string Title { get; init; }
    public ApiResult<System.Text.Json.JsonElement> Result { get; init; } = ApiResult<System.Text.Json.JsonElement>.Success(default);
    public IReadOnlyList<TripRowViewModel> Trips { get; init; } = Array.Empty<TripRowViewModel>();
}

public sealed class TripRowViewModel
{
    public int? Id { get; init; }
    public string Route { get; init; } = "Not provided";
    public string Bus { get; init; } = "Not provided";
    public string? TripDate { get; init; }
    public string? DepartureTime { get; init; }
    public string? ArrivalTime { get; init; }
    public string Driver1 { get; init; } = "Not provided";
    public string Driver2 { get; init; } = "Not provided";
    public string? Fare { get; init; }
    public string? AvailableSeats { get; init; }
}
