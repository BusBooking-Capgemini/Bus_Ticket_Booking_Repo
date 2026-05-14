using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_Bus_Ticket_Booking.Helpers
{
    public static class TestHelper
    {
        public static dynamic GetResponseData(IActionResult result)
        {
            var okResult = result as OkObjectResult;

            return okResult?.Value;
        }
    }
}
