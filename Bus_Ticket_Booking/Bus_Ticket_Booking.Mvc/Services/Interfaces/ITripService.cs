using Bus_Ticket_Booking.Mvc.ViewModels.Trip;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bus_Ticket_Booking.Mvc.Services.Interfaces
{
    public interface ITripService
    {
        // =========================
        // CUSTOMER
        // =========================

        Task<List<TripViewModel>>
            GetAllTripsAsync();

        Task<List<TripViewModel>>
            SearchTripsAsync(
                TripSearchViewModel model);

        Task<SeatMapViewModel?>
            GetSeatMapAsync(
                int tripId);


        // =========================
        // OFFICE
        // =========================

        Task<List<TripViewModel>>
            GetOfficeTripsAsync(
                string token);

        Task<TripViewModel?>
            GetTripByIdAsync(
                int tripId);

        Task<bool>
            CreateTripAsync(
                CreateTripViewModel model,
                string token);

        Task<bool>
            UpdateTripAsync(
                UpdateTripViewModel model,
                string token);

        Task<bool>
            DeleteTripAsync(
                int tripId,
                string token);


        // =========================
        // DROPDOWNS
        // =========================

        Task<List<SelectListItem>>
            GetRoutesDropdownAsync(
                string token);

        Task<List<SelectListItem>>
            GetBusesDropdownAsync(
                string token);

        Task<List<SelectListItem>>
            GetAddressesDropdownAsync(
                string token);

        Task<List<SelectListItem>>
            GetDriversDropdownAsync(
                string token);
    }
}