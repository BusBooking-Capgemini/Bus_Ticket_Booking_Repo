using API_Bus_Ticket_Booking.DTOs.Customer;
using API_Bus_Ticket_Booking.Exceptions;
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
        if (customerId <= 0)
            throw new BadRequestException("Invalid customer id");

        var customer = await _customerRepo.GetByIdAsync(customerId);

        if (customer == null)
            throw new NotFoundException("Customer not found");

        return MapToResponseDto(customer);
    }

    public async Task<CustomerResponseDto> CreateCustomerAsync(CustomerRequestDto dto)
    {
        if (dto == null)
            throw new BadRequestException("Customer data is required");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException("Customer name is required");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ValidationException("Email is required");

        // Check duplicate email
        var existingCustomer = await _customerRepo.GetByEmailAsync(dto.Email);

        if (existingCustomer != null)
            throw new ConflictException("Email already exists");

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
            Address = address,
        };

        var created = await _customerRepo.CreateAsync(customer);

        if (created == null)
            throw new BusinessException("Customer creation failed");

        return MapToResponseDto(created);
    }

    public async Task<bool>
    UpdateCustomerAsync(
        int customerId,
        CustomerRequestDto dto)
    {
        if (customerId <= 0)
        {
            throw new BadRequestException(
                "Invalid customer id");
        }

        if (dto == null)
        {
            throw new BadRequestException(
                "Update data is required");
        }

        var customer =
            await _customerRepo
                .GetByIdAsync(customerId);

        if (customer == null)
        {
            throw new NotFoundException(
                "Customer not found");
        }

        // EMAIL

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            var existingCustomer =
                await _customerRepo
                    .GetByEmailAsync(dto.Email);

            if (existingCustomer != null &&
                existingCustomer.CustomerId != customerId)
            {
                throw new ConflictException(
                    "Email already exists");
            }

            customer.Email = dto.Email;
        }

        // BASIC INFO

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            customer.Name = dto.Name;
        }

        if (!string.IsNullOrWhiteSpace(dto.Phone))
        {
            customer.Phone = dto.Phone;
        }

        // ADDRESS CREATE OR UPDATE

        if (customer.Address == null)
        {
            customer.Address = new Address
            {
                Address1 = dto.Address ?? "",
                City = dto.City ?? "",
                State = dto.State ?? "",
                ZipCode = dto.ZipCode ?? ""
            };
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(dto.Address))
            {
                customer.Address.Address1 =
                    dto.Address;
            }

            if (!string.IsNullOrWhiteSpace(dto.City))
            {
                customer.Address.City =
                    dto.City;
            }

            if (!string.IsNullOrWhiteSpace(dto.State))
            {
                customer.Address.State =
                    dto.State;
            }

            if (!string.IsNullOrWhiteSpace(dto.ZipCode))
            {
                customer.Address.ZipCode =
                    dto.ZipCode;
            }
        }

        await _customerRepo
            .UpdateAsync(customer);

        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int customerId)
    {
        if (customerId <= 0)
            throw new BadRequestException("Invalid customer id");

        var customer = await _customerRepo.GetByIdAsync(customerId);

        if (customer == null)
            throw new NotFoundException("Customer not found");

        await _customerRepo.DeleteAsync(customer);

        return true;
    }

    public async Task<bool> EmailAlreadyExistsAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("Email is required");

        var existingCustomer = await _customerRepo.GetByEmailAsync(email);

        return existingCustomer != null;
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
