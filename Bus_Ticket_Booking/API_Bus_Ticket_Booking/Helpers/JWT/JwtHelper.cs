using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Bus_Ticket_Booking.Helpers.JWT
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(
            string userId,
            string email,
            string role)
        {
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    userId),

                new Claim(
                    ClaimTypes.Email,
                    email),

                new Claim(
                    ClaimTypes.Role,
                    role)
            };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["JWT_KEY"]!));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var expiryMinutes =
                Convert.ToDouble(
                    _configuration["JWT_DURATION_IN_MINUTES"]);

            var token =
                new JwtSecurityToken(
                    issuer:
                        _configuration["JWT_ISSUER"],

                    audience:
                        _configuration["JWT_AUDIENCE"],

                    claims: claims,

                    expires:
                        DateTime.Now.AddMinutes(
                            expiryMinutes),

                    signingCredentials:
                        credentials
                );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}