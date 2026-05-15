using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace TEST_Bus_Ticket_Booking.Helpers
{
    public static class TestHelper
    {
        public static dynamic GetResponseData(IActionResult result)
        {
            var okResult = result as OkObjectResult;

            return okResult?.Value;
        }

        public static T GetPropertyValue<T>(object obj, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

            var property = obj.GetType().GetProperty(
                propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (property == null)
            {
                throw new InvalidOperationException(
                    $"Property '{propertyName}' was not found on type '{obj.GetType().Name}'.");
            }

            var value = property.GetValue(obj);

            if (value is null)
            {
                return default!;
            }

            if (value is T typedValue)
            {
                return typedValue;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static ClaimsPrincipal CreatePrincipal(int userId, string role)
        {
            return CreatePrincipal(userId.ToString(), role);
        }

        public static ClaimsPrincipal CreatePrincipal(string userId, string role)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, authenticationType: "Test");

            return new ClaimsPrincipal(identity);
        }
    }
}