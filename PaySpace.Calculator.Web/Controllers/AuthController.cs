using Microsoft.AspNetCore.Mvc;
using PaySpace.Calculator.Web.Services.Abstractions;
using System.Text.Json;
using System.Text;
using PaySpace.Calculator.Web.Models;

namespace PaySpace.Calculator.Web.Controllers;

public sealed class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ISessionManager _sessionManager;

    public AuthController(IAuthService authService, ISessionManager sessionManager)
    {
        _authService = authService;
        _sessionManager = sessionManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        var model = new LoginViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var loginRequest = new
        {
            Email = model.Email,
            Password = model.Password
        };

        var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

        var response = await _authService.LoginAsync(content);
        if (response.IsSuccessStatusCode)
        {
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

            _sessionManager.SetToken(tokenResponse.Token);

            return Redirect(returnUrl ?? "/");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    [HttpPost]
    public IActionResult Logout()
    {
        _sessionManager.ClearToken();
        return RedirectToAction("Login", "Auth");
    }
}

public class TokenResponse
{
    public string Token { get; set; } = string.Empty;
}
