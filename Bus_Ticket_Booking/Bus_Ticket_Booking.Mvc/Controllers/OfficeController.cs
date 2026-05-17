using Bus_Ticket_Booking.Mvc.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_Booking.Mvc.Controllers
{
    public class OfficeController : Controller
    {
        public IActionResult Dashboard()
        {
            var token =
                HttpContext.Session.GetString(
                    SessionKeys.Token);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "OfficeLogin",
                    "Auth");
            }

            var role =
                JwtHelper.GetRole(token);

            if (role != "Office")
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            ViewBag.OfficeId =
                JwtHelper.GetClaim(
                    token,
                    "OfficeId");

            ViewBag.Email =
                JwtHelper.GetEmail(token);

            return View();
        }
    }
}