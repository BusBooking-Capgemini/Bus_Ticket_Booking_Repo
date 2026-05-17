using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Trip;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class TripController : Controller
    {
        private readonly ITripService _tripService;

        public TripController(
            ITripService tripService)
        {
            _tripService = tripService;
        }

        public async Task<IActionResult>
            Index()
        {
            var trips =
                await _tripService
                    .GetAllTripsAsync();

            return View(trips);
        }

        [HttpPost]
        public async Task<IActionResult>
            Search(
                TripSearchViewModel model)
        {
            var trips =
                await _tripService
                    .SearchTripsAsync(model);

            return View(
                "Index",
                trips);
        }

        public async Task<IActionResult>
    Seats(int id)
        {
            var seatMap =
                await _tripService
                    .GetSeatMapAsync(id);

            if (seatMap == null)
            {
                return NotFound();
            }

            return View(seatMap);
        }
    }
}