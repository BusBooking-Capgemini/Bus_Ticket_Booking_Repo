using API_Bus_Ticket_Booking.DTOs.Customer;

namespace API_Bus_Ticket_Booking.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponseDto?> GetCustomerAsync(int customerId);
    Task<CustomerResponseDto> CreateCustomerAsync(CustomerRequestDto dto);
    Task<bool> UpdateCustomerAsync(int customerId, CustomerRequestDto dto);
    Task<bool> DeleteCustomerAsync(int customerId);
    Task<bool> EmailAlreadyExistsAsync(string email);
}
