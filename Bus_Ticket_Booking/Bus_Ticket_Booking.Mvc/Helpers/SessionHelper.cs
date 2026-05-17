using Microsoft.AspNetCore.Http;

namespace Bus_Ticket_Booking.Mvc.Helpers
{
    public static class SessionHelper
    {
        public static void
            SetUserSession(
                HttpContext context,
                string token)
        {
            context.Session.SetString(
                SessionKeys.Token,
                token);

            var role =
                JwtHelper.GetRole(token);

            var email =
                JwtHelper.GetEmail(token);

            var userId =
                JwtHelper.GetUserId(token);

            if (!string.IsNullOrEmpty(role))
            {
                context.Session.SetString(
                    SessionKeys.Role,
                    role);
            }

            if (!string.IsNullOrEmpty(email))
            {
                context.Session.SetString(
                    SessionKeys.Email,
                    email);
            }

            if (!string.IsNullOrEmpty(userId))
            {
                context.Session.SetString(
                    SessionKeys.UserId,
                    userId);
            }
        }

        public static void
            ClearSession(HttpContext context)
        {
            context.Session.Clear();
        }
    }
}