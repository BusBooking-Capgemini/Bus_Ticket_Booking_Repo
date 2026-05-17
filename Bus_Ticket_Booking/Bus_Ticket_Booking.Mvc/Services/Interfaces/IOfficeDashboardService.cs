using Bus_Ticket_Booking.Mvc.ViewModels.OfficeDashboard;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface IOfficeDashboardService
    {
        Task<OfficeDashboardViewModel?>
            GetDashboardAsync(
                int officeId,
                string token);
    }
}