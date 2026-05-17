using Bus_Ticket_Booking.Mvc.ViewModels.AgencyDashboard;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface IAgencyDashboardService
    {
        

        Task<AgencyDashboardViewModel?>
            GetDashboardAsync(
                int agencyId,
                string token);

        

        Task<AgencyOfficeDetailsViewModel?>
            GetOfficeDetailsAsync(
                int agencyId,
                int officeId,
                string token);
    }
}