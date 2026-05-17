using Bus_Ticket_Booking.Mvc.ViewModels.Driver;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface IDriverService
    {
        Task<List<DriverViewModel>>
            GetOfficeDriversAsync(
                string token);

        Task<DriverViewModel?>
            GetDriverByIdAsync(
                int id,
                string token);

        Task<bool>
            CreateDriverAsync(
                CreateDriverViewModel model,
                string token);

        Task<bool>
            UpdateDriverAsync(
                UpdateDriverViewModel model,
                string token);

        Task<bool>
            DeleteDriverAsync(
                int id,
                string token);
    }
}