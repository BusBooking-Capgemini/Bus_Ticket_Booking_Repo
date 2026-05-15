using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusTicketBooking.Mvc.Helpers;

public static class JsonLookupHelper
{
    public static IReadOnlyDictionary<int, string> BuildLookup(JsonElement payload, string type)
    {
        return JsonDisplayHelper.GetRows(payload)
            .Select(row => new { Id = GetInt(row, "id", $"{type}Id"), Label = GetLabel(row, type) })
            .Where(item => item.Id.HasValue)
            .GroupBy(item => item.Id!.Value)
            .ToDictionary(group => group.Key, group => group.First().Label);
    }

    public static IEnumerable<SelectListItem> BuildSelectList(JsonElement payload, string type, int? selectedValue = null)
    {
        return BuildLookup(payload, type)
            .OrderBy(item => item.Value)
            .Select(item => new SelectListItem
            {
                Value = item.Key.ToString(),
                Text = item.Value,
                Selected = selectedValue == item.Key
            });
    }

    public static int? GetInt(JsonElement element, params string[] names)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        foreach (var property in element.EnumerateObject())
        {
            if (!names.Any(name => string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            if (property.Value.ValueKind == JsonValueKind.Number && property.Value.TryGetInt32(out var number))
            {
                return number;
            }

            if (property.Value.ValueKind == JsonValueKind.String && int.TryParse(property.Value.GetString(), out number))
            {
                return number;
            }
        }

        return null;
    }

    public static string? GetString(JsonElement element, params string[] names)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        foreach (var property in element.EnumerateObject())
        {
            if (names.Any(name => string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                return JsonDisplayHelper.ToDisplay(property.Value);
            }
        }

        return null;
    }

    public static JsonElement? GetObject(JsonElement element, params string[] names)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        foreach (var property in element.EnumerateObject())
        {
            if (names.Any(name => string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase)) &&
                property.Value.ValueKind == JsonValueKind.Object)
            {
                return property.Value;
            }
        }

        return null;
    }

    public static string GetLabel(JsonElement row, string type)
    {
        return type switch
        {
            "route" => FormatRoute(row),
            "bus" => FormatBus(row),
            "driver" => FormatDriver(row),
            _ => GetString(row, "name", "title", "email") ?? $"#{GetInt(row, "id")?.ToString() ?? "unknown"}"
        };
    }

    private static string FormatRoute(JsonElement row)
    {
        var from = GetString(row, "fromCity");
        var to = GetString(row, "toCity");
        var duration = GetString(row, "duration");
        var route = string.Join(" to ", new[] { from, to }.Where(value => !string.IsNullOrWhiteSpace(value)));
        if (string.IsNullOrWhiteSpace(route))
        {
            route = $"Route #{GetInt(row, "id", "routeId")?.ToString() ?? "unknown"}";
        }

        return string.IsNullOrWhiteSpace(duration) ? route : $"{route} ({duration} min)";
    }

    private static string FormatBus(JsonElement row)
    {
        var reg = GetString(row, "registrationNumber", "regNumber");
        var type = GetString(row, "type");
        var capacity = GetString(row, "capacity");
        var bus = string.Join(" - ", new[] { reg, type }.Where(value => !string.IsNullOrWhiteSpace(value)));
        if (string.IsNullOrWhiteSpace(bus))
        {
            bus = $"Bus #{GetInt(row, "id", "busId")?.ToString() ?? "unknown"}";
        }

        return string.IsNullOrWhiteSpace(capacity) ? bus : $"{bus} ({capacity} seats)";
    }

    private static string FormatDriver(JsonElement row)
    {
        var name = GetString(row, "name");
        var license = GetString(row, "licenseNumber");
        var driver = string.Join(" - ", new[] { name, license }.Where(value => !string.IsNullOrWhiteSpace(value)));
        return string.IsNullOrWhiteSpace(driver) ? $"Driver #{GetInt(row, "id", "driverId")?.ToString() ?? "unknown"}" : driver;
    }
}
