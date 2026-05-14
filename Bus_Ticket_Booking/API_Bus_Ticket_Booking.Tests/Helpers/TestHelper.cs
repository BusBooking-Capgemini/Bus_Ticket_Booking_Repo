using System.Reflection;
using System.Security.Claims;

namespace TEST_Bus_Ticket_Booking.Helpers;

public static class TestHelper
{
    public static T? GetPropertyValue<T>(object source, string propertyName)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));
        }

        var property = source
            .GetType()
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

        if (property is null)
        {
            return default;
        }

        var value = property.GetValue(source);
        if (value is null)
        {
            return default;
        }

        return value is T typedValue ? typedValue : (T)Convert.ChangeType(value, typeof(T));
    }

    public static ClaimsPrincipal CreatePrincipal(int userId, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Role, role),
        };

        var identity = new ClaimsIdentity(claims, authenticationType: "TestAuthType");
        return new ClaimsPrincipal(identity);
    }
}