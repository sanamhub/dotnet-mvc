using System.Security.Claims;
using App.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    public IActionResult Login()
    {
        _logger.LogInformation("Landed to login page");

        if (HttpContext.User?.Identity.IsAuthenticated == true)
        {
            _logger.LogInformation("User already authenticated, redirecting");
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        _logger.LogInformation("Login initiated by {Email}", vm.Email);

        // authenticate (temp for now)
        if (vm.Email == "sa@sa.sa" && vm.Password == "123")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, vm.Email),
                new Claim("Props", "ExampleRole")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = vm.RememberMe
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

            _logger.LogInformation("Login successful, redirecting");

            return RedirectToAction("Index", "Home");
        }

        _logger.LogWarning("Login not successful");

        ViewData["ValidationMessage"] = "Invalid email or password";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        _logger.LogInformation("Logout initiated by {email}", email);

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        _logger.LogInformation("Logout successful");

        return RedirectToAction(nameof(Login));
    }
}
