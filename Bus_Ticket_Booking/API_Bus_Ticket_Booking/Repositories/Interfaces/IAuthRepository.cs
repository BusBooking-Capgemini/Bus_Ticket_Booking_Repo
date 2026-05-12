using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        // CUSTOMER

        Task<Customer?> GetCustomerByEmailAsync(
            string email);

        Task<bool> CustomerEmailExistsAsync(
            string email);

        Task<Customer> CreateCustomerAsync(
            Customer customer);

        // AGENCY

        Task<Agency?> GetAgencyByEmailAsync(
            string email);

        // OFFICE

        Task<AgencyOffice?> GetOfficeByEmailAsync(
            string email);

        // ROLE

        Task<Role?> GetRoleByNameAsync(
            string roleName);
    }
}