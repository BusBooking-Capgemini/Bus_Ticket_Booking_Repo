using System.Text.Json;
using BusTicketBooking.Mvc.ViewModels;

namespace BusTicketBooking.Mvc.Helpers;

public static class TripDisplayHelper
{
    public static IReadOnlyList<TripRowViewModel> BuildRows(
        JsonElement payload,
        IReadOnlyDictionary<int, string> routeLookup,
        IReadOnlyDictionary<int, string> busLookup,
        IReadOnlyDictionary<int, string> driverLookup)
    {
        return JsonDisplayHelper.GetRows(payload).Select(row => BuildRow(row, routeLookup, busLookup, driverLookup)).ToList();
    }

    private static TripRowViewModel BuildRow(
        JsonElement row,
        IReadOnlyDictionary<int, string> routeLookup,
        IReadOnlyDictionary<int, string> busLookup,
        IReadOnlyDictionary<int, string> driverLookup)
    {
        var routeId = JsonLookupHelper.GetInt(row, "routeId");
        var busId = JsonLookupHelper.GetInt(row, "busId");
        var driver1Id = JsonLookupHelper.GetInt(row, "driver1DriverId", "driver1Id");
        var driver2Id = JsonLookupHelper.GetInt(row, "driver2DriverId", "driver2Id");

        return new TripRowViewModel
        {
            Id = JsonLookupHelper.GetInt(row, "id", "tripId"),
            Route = GetNestedLabel(row, "route", "route") ?? Lookup(routeLookup, routeId, "Route"),
            Bus = GetNestedLabel(row, "bus", "bus") ?? Lookup(busLookup, busId, "Bus"),
            DepartureTime = JsonLookupHelper.GetString(row, "departureTime"),
            ArrivalTime = JsonLookupHelper.GetString(row, "arrivalTime"),
            TripDate = JsonLookupHelper.GetString(row, "tripDate", "date"),
            Driver1 = GetNestedLabel(row, "driver1", "driver") ?? Lookup(driverLookup, driver1Id, "Driver"),
            Driver2 = GetNestedLabel(row, "driver2", "driver") ?? Lookup(driverLookup, driver2Id, "Driver"),
            Fare = JsonLookupHelper.GetString(row, "fare", "price", "amount"),
            AvailableSeats = JsonLookupHelper.GetString(row, "availableSeats", "seatsAvailable", "remainingSeats")
        };
    }

    private static string? GetNestedLabel(JsonElement row, string propertyName, string type)
    {
        var nested = JsonLookupHelper.GetObject(row, propertyName);
        return nested.HasValue ? JsonLookupHelper.GetLabel(nested.Value, type) : null;
    }

    private static string Lookup(IReadOnlyDictionary<int, string> lookup, int? id, string fallback)
    {
        if (id.HasValue && lookup.TryGetValue(id.Value, out var value))
        {
            return value;
        }

        return id.HasValue ? $"{fallback} #{id.Value}" : "Not provided";
    }
}
