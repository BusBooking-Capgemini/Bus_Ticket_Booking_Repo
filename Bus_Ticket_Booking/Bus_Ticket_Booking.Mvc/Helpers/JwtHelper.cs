using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bus_Ticket_Booking.Mvc.Helpers
{
    public static class JwtHelper
    {
        public static string?
            GetClaim(
                string token,
                string claimType)
        {
            var handler =
                new JwtSecurityTokenHandler();

            var jwtToken =
                handler.ReadJwtToken(token);

            return jwtToken.Claims
                .FirstOrDefault(c =>
                    c.Type == claimType)
                ?.Value;
        }

        public static string?
            GetUserId(string token)
        {
            return GetClaim(
                token,
                ClaimTypes.NameIdentifier);
        }

        public static string?
            GetRole(string token)
        {
            return GetClaim(
                token,
                ClaimTypes.Role);
        }

        public static string?
            GetEmail(string token)
        {
            return GetClaim(
                token,
                ClaimTypes.Email);
        }

        public static string?
            GetOfficeId(string token)
        {
            return GetClaim(
                token,
                "OfficeId");
        }

        public static string?
    GetAgencyId(string token)
        {
            return GetClaim(
                token,
                "AgencyId");
        }
    }
}