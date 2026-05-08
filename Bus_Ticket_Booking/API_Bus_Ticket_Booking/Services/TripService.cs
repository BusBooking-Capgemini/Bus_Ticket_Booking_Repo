using API_Bus_Ticket_Booking.DTOs.Trip;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;

namespace API_Bus_Ticket_Booking.Services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepo;

    public TripService(ITripRepository tripRepo)
    {
        _tripRepo = tripRepo;
    }

    public async Task<List<TripResponseDto>> SearchTripsAsync(TripSearchDto filters)
    {
        var trips = await _tripRepo.SearchTripsAsync(
            filters.FromCity,
            filters.ToCity,
            filters.TripDate,
            filters.MinSeats,
            filters.MaxFare
        );

        return trips.Select(MapToDto).ToList();
    }

    public async Task<TripResponseDto?> GetTripDetailsAsync(int tripId)
    {
        var trip = await _tripRepo.GetByIdAsync(tripId);
        return trip == null ? null : MapToDto(trip);
    }

    public async Task<List<TripResponseDto>> GetUpcomingTripsAsync()
    {
        var trips = await _tripRepo.GetUpcomingTripsAsync();
        return trips.Select(MapToDto).ToList();
    }

    public async Task<List<TripResponseDto>> GetTripsByRouteAsync(int routeId)
    {
        var trips = await _tripRepo.GetTripsByRouteAsync(routeId);
        return trips.Select(MapToDto).ToList();
    }

    private static TripResponseDto MapToDto(Trip t) =>
        new()
        {
            TripId = t.TripId,
            FromCity = t.Route?.FromCity ?? "",
            ToCity = t.Route?.ToCity ?? "",
            TripDate = t.TripDate,
            DepartureTime = t.DepartureTime,
            ArrivalTime = t.ArrivalTime,
            AvailableSeats = t.AvailableSeats,
            Fare = t.Fare,
            BusType = t.Bus?.Type ?? "",
            BreakPoints = t.Route?.BreakPoints,
            Duration = t.Route?.Duration,
        };
}
