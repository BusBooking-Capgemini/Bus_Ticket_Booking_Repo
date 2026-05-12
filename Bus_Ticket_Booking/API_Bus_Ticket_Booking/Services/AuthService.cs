using API_Bus_Ticket_Booking.DTOs.Auth;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Helpers.JWT;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;

namespace API_Bus_Ticket_Booking.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        private readonly JwtHelper _jwtHelper;

        public AuthService(
            IAuthRepository authRepository,
            JwtHelper jwtHelper)
        {
            _authRepository = authRepository;

            _jwtHelper = jwtHelper;
        }

        // CUSTOMER SIGNUP

        public async Task<AuthResponseDto>
            CustomerSignupAsync(
                CustomerSignupDto dto)
        {
            bool emailExists =
                await _authRepository
                    .CustomerEmailExistsAsync(dto.Email);

            if (emailExists)
            {
                throw new ConflictException(
                    "Email already exists");
            }

            var customerRole =
                await _authRepository
                    .GetRoleByNameAsync("Customer");

            if (customerRole == null)
            {
                throw new NotFoundException(
                    "Customer role not found");
            }

            string hashedPassword =
                BCrypt.Net.BCrypt.HashPassword(
                    dto.Password);

            var customer = new Customer
            {
                Name = dto.Name,

                Email = dto.Email,

                Phone = dto.Phone,

                PasswordHash = hashedPassword,

                RoleId = customerRole.RoleId
            };

            var createdCustomer =
                await _authRepository
                    .CreateCustomerAsync(customer);

            var token =
                _jwtHelper.GenerateToken(
                    createdCustomer.CustomerId.ToString(),
                    createdCustomer.Email,
                    customerRole.RoleName);

            return new AuthResponseDto
            {
                Token = token,

                Email = createdCustomer.Email,

                Role = customerRole.RoleName
            };
        }

        // CUSTOMER LOGIN

        public async Task<AuthResponseDto>
            CustomerLoginAsync(
                LoginRequestDto dto)
        {
            var customer =
                await _authRepository
                    .GetCustomerByEmailAsync(dto.Email);

            if (customer == null)
            {
                throw new UnauthorizedException(
                    "Invalid email or password");
            }

            bool isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    customer.PasswordHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedException(
                    "Invalid email or password");
            }

            var token =
                _jwtHelper.GenerateToken(
                    customer.CustomerId.ToString(),
                    customer.Email,
                    customer.Role!.RoleName);

            return new AuthResponseDto
            {
                Token = token,

                Email = customer.Email,

                Role = customer.Role.RoleName
            };
        }

        // AGENCY LOGIN

        public async Task<AuthResponseDto>
            AgencyLoginAsync(
                LoginRequestDto dto)
        {
            var agency =
                await _authRepository
                    .GetAgencyByEmailAsync(dto.Email);

            if (agency == null)
            {
                throw new UnauthorizedException(
                    "Invalid email or password");
            }

            bool isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    agency.PasswordHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedException(
                    "Invalid email or password");
            }

            var token =
                _jwtHelper.GenerateToken(
                    agency.AgencyId.ToString(),
                    agency.Email,
                    agency.Role!.RoleName);

            return new AuthResponseDto
            {
                Token = token,

                Email = agency.Email,

                Role = agency.Role.RoleName
            };
        }

        // OFFICE LOGIN

        public async Task<AuthResponseDto>
            OfficeLoginAsync(
                LoginRequestDto dto)
        {
            var office =
                await _authRepository
                    .GetOfficeByEmailAsync(dto.Email);

            if (office == null)
            {
                throw new UnauthorizedException(
                    "Invalid email or password");
            }

            bool isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    office.PasswordHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedException(
                    "Invalid email or password");
            }

            var token =
                _jwtHelper.GenerateToken(
                    office.OfficeId.ToString(),
                    office.OfficeMail!,
                    office.Role!.RoleName);

            return new AuthResponseDto
            {
                Token = token,

                Email = office.OfficeMail!,

                Role = office.Role.RoleName
            };
        }
    }
}