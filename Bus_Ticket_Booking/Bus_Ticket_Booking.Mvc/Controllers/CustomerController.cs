using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // DASHBOARD

        public IActionResult Dashboard()
        {
            return View();
        }

        // PROFILE PAGE

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await _customerService.GetProfileAsync(token);

            if (model == null)
            {
                return RedirectToAction("Dashboard");
            }

            return View(model);
        }

        // UPDATE PROFILE

        [HttpPost]
        public async Task<IActionResult> Profile(CustomerProfileViewModel model)
        {
            var token = HttpContext.Session.GetString(SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _customerService.UpdateProfileAsync(model, token);

            if (!success)
            {
                ViewBag.Error = "Profile update failed";

                return View(model);
            }

            ViewBag.Success = "Profile updated successfully";

            return View(model);
        }
    }
}
