using Bus_Ticket_Booking.Mvc.ViewModels.Bus;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface IBusService
    {
        

        Task<List<BusViewModel>>
            GetOfficeBusesAsync(
                string token);



        Task<BusViewModel?>
    GetBusByIdAsync(
        int id,
        string token);



        Task<bool>
            CreateBusAsync(
                CreateBusViewModel model,
                string token);

        

        Task<bool>
            UpdateBusAsync(
                UpdateBusViewModel model,
                string token);

        

        Task<bool>
            DeleteBusAsync(
                int id,
                string token);
    }
}