using Bus_Ticket_Booking.Mvc.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bus_Ticket_Booking.Mvc.Filters
{
    public class AgencyAuthorizeAttribute
        : ActionFilterAttribute
    {
        public override void
            OnActionExecuting(
                ActionExecutingContext context)
        {
            var role =
                context.HttpContext.Session
                    .GetString(SessionKeys.Role);

            if (role != "Agency")
            {
                context.Result =
                    new RedirectToActionResult(
                        "Login",
                        "Auth",
                        null);
            }

            base.OnActionExecuting(context);
        }
    }
}