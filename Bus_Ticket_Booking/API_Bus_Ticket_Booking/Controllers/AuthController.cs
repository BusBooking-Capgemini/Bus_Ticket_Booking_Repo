using API_Bus_Ticket_Booking.DTOs.Auth;
using API_Bus_Ticket_Booking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Bus_Ticket_Booking.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }

        // CUSTOMER SIGNUP
        [HttpPost("customer-signup")]
        public async Task<IActionResult> CustomerSignup(
            [FromBody] CustomerSignupDto dto)
        {
            var result =
                await _authService
                    .CustomerSignupAsync(dto);

            return Ok(result);
        }

        // CUSTOMER LOGIN
        [HttpPost("customer-login")]
        public async Task<IActionResult> CustomerLogin(
            [FromBody] LoginRequestDto dto)
        {
            var result =
                await _authService
                    .CustomerLoginAsync(dto);

            return Ok(result);
        }

        // AGENCY LOGIN
        [HttpPost("agency-login")]
        public async Task<IActionResult> AgencyLogin(
            [FromBody] LoginRequestDto dto)
        {
            var result =
                await _authService
                    .AgencyLoginAsync(dto);

            return Ok(result);
        }

        // OFFICE LOGIN
        [HttpPost("office-login")]
        public async Task<IActionResult> OfficeLogin(
            [FromBody] LoginRequestDto dto)
        {
            var result =
                await _authService
                    .OfficeLoginAsync(dto);

            return Ok(result);
        }
    }
}