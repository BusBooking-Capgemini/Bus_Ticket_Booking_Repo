using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusTicketBooking.Mvc.Pages.Status;

public sealed class AboutModel(IConfiguration configuration) : PageModel
{
    public string ApiBaseUrl { get; private set; } = string.Empty;

    public void OnGet()
    {
        ApiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "Not configured";
    }
}
