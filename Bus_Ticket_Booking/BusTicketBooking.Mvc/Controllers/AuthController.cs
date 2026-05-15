using System.Text.Json;
using BusTicketBooking.Mvc.Helpers;
using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BusTicketBooking.Mvc.Controllers;

public sealed class AuthController(IAuthService authService) : Controller
{
    [HttpGet]
    public IActionResult Login(string role = "Customer") => View(new LoginViewModel { Role = role });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await authService.LoginAsync(model);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, GetLoginError(model.Role, result.Message));
            return View(model);
        }

        HttpContext.Session.SetString(SessionKeys.Role, model.Role);
        HttpContext.Session.SetString(SessionKeys.Email, model.Email ?? string.Empty);

        if (TryFindString(result.Data, out var token, "token", "accessToken", "jwt"))
        {
            HttpContext.Session.SetString(SessionKeys.Token, token);
        }

        SetIfFound(result.Data, SessionKeys.DisplayName, "name", "displayName", "agencyName", "officeContactPersonName");
        SetIfFound(result.Data, SessionKeys.UserId, "id", "userId");
        SetIfFound(result.Data, SessionKeys.CustomerId, "customerId");
        SetIfFound(result.Data, SessionKeys.AgencyId, "agencyId");
        SetIfFound(result.Data, SessionKeys.OfficeId, "officeId");

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public IActionResult CustomerRegister() => View(new CustomerSignupViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CustomerRegister(CustomerSignupViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await authService.RegisterCustomerAsync(model);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed.");
            return View(model);
        }

        TempData["StatusMessage"] = "Customer registration completed. Please sign in.";
        return RedirectToAction(nameof(Login), new { role = "Customer" });
    }

    [HttpGet]
    public IActionResult AgencyRegister()
    {
        ViewData["Title"] = "Agency Register";
        return View("EndpointUnavailable", "No agency registration endpoint was provided. The API only exposes agency update, delete, details, offices, and summary endpoints.");
    }

    [HttpGet]
    public IActionResult OfficeRegister() => RedirectToAction("Create", "Offices");

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }

    private static string GetLoginError(string role, string? apiMessage)
    {
        var selectedRole = string.IsNullOrWhiteSpace(role) ? "selected role" : role.ToLowerInvariant();
        if (apiMessage?.Contains("401", StringComparison.OrdinalIgnoreCase) == true ||
            apiMessage?.Contains("403", StringComparison.OrdinalIgnoreCase) == true ||
            apiMessage?.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) == true)
        {
            return $"These credentials are not valid for {selectedRole} login. Please choose the correct login type or use the account created for that role.";
        }

        return string.IsNullOrWhiteSpace(apiMessage)
            ? $"Login failed for {selectedRole}. Please check the login type, email, and password."
            : $"Login failed for {selectedRole}. {apiMessage}";
    }

    private void SetIfFound(JsonElement? payload, string sessionKey, params string[] names)
    {
        if (TryFindString(payload, out var value, names))
        {
            HttpContext.Session.SetString(sessionKey, value);
        }
    }

    private static bool TryFindString(JsonElement? payload, out string value, params string[] names)
    {
        value = string.Empty;
        if (payload is null || payload.Value.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        return TryFindString(payload.Value, out value, names);
    }

    private static bool TryFindString(JsonElement payload, out string value, params string[] names)
    {
        value = string.Empty;

        if (payload.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        foreach (var property in payload.EnumerateObject())
        {
            if (names.Any(name => string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                value = property.Value.ValueKind == JsonValueKind.String
                    ? property.Value.GetString() ?? string.Empty
                    : property.Value.ToString();
                return !string.IsNullOrWhiteSpace(value);
            }

            if (property.Value.ValueKind == JsonValueKind.Object && TryFindString(property.Value, out value, names))
            {
                return true;
            }
        }

        return false;
    }
}
