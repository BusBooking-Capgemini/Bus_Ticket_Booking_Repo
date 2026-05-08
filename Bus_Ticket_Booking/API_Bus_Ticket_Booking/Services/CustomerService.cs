using API_Bus_Ticket_Booking.DTOs.Customer;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;

namespace API_Bus_Ticket_Booking.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepo;

    public CustomerService(ICustomerRepository repo)
    {
        _customerRepo = repo;
    }

    public async Task<CustomerResponseDto?> GetCustomerAsync(int customerId)
    {
        var c = await _customerRepo.GetByIdAsync(customerId);

        if (c == null)
            return null;

        return MapToResponseDto(c);
    }

    public async Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateDto dto)
    {
        var address = new Address
        {
            Address1 = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
        };

        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = address, // EF handles address insert + FK
        };

        var created = await _customerRepo.CreateAsync(customer);

        return MapToResponseDto(created);
    }

    public async Task<bool> UpdateCustomerAsync(int customerId, CustomerUpdateDto dto)
    {
        var customer = await _customerRepo.GetByIdAsync(customerId);

        if (customer == null)
            return false;

        if (dto.Name != null)
            customer.Name = dto.Name;
        if (dto.Email != null)
            customer.Email = dto.Email;
        if (dto.Phone != null)
            customer.Phone = dto.Phone;

        if (customer.Address != null)
        {
            if (dto.Address != null)
                customer.Address.Address1 = dto.Address;
            if (dto.City != null)
                customer.Address.City = dto.City;
            if (dto.State != null)
                customer.Address.State = dto.State;
            if (dto.ZipCode != null)
                customer.Address.ZipCode = dto.ZipCode;
        }

        await _customerRepo.UpdateAsync(customer);
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        var customer = await _customerRepo.GetByIdAsync(customerId);

        if (customer == null)
            return false;

        await _customerRepo.DeleteAsync(customer);
        return true;
    }

    public async Task<bool> EmailAlreadyExistsAsync(string email)
    {
        var exist = await _customerRepo.GetByEmailAsync(email);

        if (exist == null)
            return false;

        return true;
    }

    private static CustomerResponseDto MapToResponseDto(Customer c) =>
        new()
        {
            CustomerId = c.CustomerId,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            Address = c.Address?.Address1,
            City = c.Address?.City,
            State = c.Address?.State,
            ZipCode = c.Address?.ZipCode,
        };
}
