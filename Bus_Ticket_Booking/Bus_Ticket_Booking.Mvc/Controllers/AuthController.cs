using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }


        // ======================
        // CUSTOMER LOGIN
        // ======================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult>
            Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result =
                await _authService
                    .CustomerLoginAsync(model);

            if (result == null)
            {
                ViewBag.Error =
                    "Invalid email or password";

                return View(model);
            }

            SessionHelper.SetUserSession(
                HttpContext,
                result.Token);

            return RedirectToAction(
                "Dashboard",
                "Customer");
        }


        // ======================
        // CUSTOMER SIGNUP
        // ======================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult>
            Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success =
                await _authService
                    .CustomerSignupAsync(model);

            if (!success)
            {
                ViewBag.Error =
                    "Registration failed";

                return View(model);
            }

            return RedirectToAction(
                "Login");
        }


        // ======================
        // AGENCY LOGIN
        // ======================

        [HttpGet]
        public IActionResult AgencyLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>
            AgencyLogin(
                AgencyLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result =
                await _authService
                    .AgencyLoginAsync(model);

            if (result == null)
            {
                ViewBag.Error =
                    "Invalid email or password.";

                return View(model);
            }

            HttpContext.Session.SetString(
                SessionKeys.Token,
                result.Token);

            HttpContext.Session.SetString(
                SessionKeys.Role,
                "Agency");

            return RedirectToAction(
                "Index",
                "AgencyDashboard");
        }


        // ======================
        // OFFICE LOGIN
        // ======================

        [HttpGet]
        public IActionResult OfficeLogin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult>
            OfficeLogin(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result =
                await _authService
                    .OfficeLoginAsync(model);

            if (result == null)
            {
                ViewBag.Error =
                    "Invalid email or password";

                return View(model);
            }

            SessionHelper.SetUserSession(
                HttpContext,
                result.Token);

            return RedirectToAction(
                "Dashboard",
                "Office");
        }

        

        // ======================
        // LOGOUT
        // ======================

        public IActionResult Logout()
        {
            SessionHelper.ClearSession(
                HttpContext);

            return RedirectToAction(
                "Login");
        }
    }
}