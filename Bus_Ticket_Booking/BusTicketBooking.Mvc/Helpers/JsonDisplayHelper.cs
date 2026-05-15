using System.Text.Json;

namespace BusTicketBooking.Mvc.Helpers;

public static class JsonDisplayHelper
{
    public static IReadOnlyList<string> GetColumns(JsonElement payload)
    {
        var first = GetRows(payload).FirstOrDefault();
        return first.ValueKind == JsonValueKind.Object
            ? first.EnumerateObject().Select(p => p.Name).Take(8).ToList()
            : new[] { "value" };
    }

    public static IReadOnlyList<JsonElement> GetRows(JsonElement payload)
    {
        if (payload.ValueKind == JsonValueKind.Array)
        {
            return payload.EnumerateArray().Take(100).ToList();
        }

        return payload.ValueKind == JsonValueKind.Undefined || payload.ValueKind == JsonValueKind.Null
            ? Array.Empty<JsonElement>()
            : new[] { payload };
    }

    public static string GetValue(JsonElement row, string column)
    {
        if (row.ValueKind != JsonValueKind.Object)
        {
            return ToDisplay(row);
        }

        return row.TryGetProperty(column, out var value) ? ToDisplay(value) : string.Empty;
    }

    public static string ToDisplay(JsonElement value)
    {
        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString() ?? string.Empty,
            JsonValueKind.Number => value.ToString(),
            JsonValueKind.True => "Yes",
            JsonValueKind.False => "No",
            JsonValueKind.Null => string.Empty,
            JsonValueKind.Undefined => string.Empty,
            _ => value.GetRawText()
        };
    }
}
