using System.Security.Claims;
using App.Base.Helpers;
using App.User.Dtos;
using App.User.Services.Interfaces;
using App.User.Validators.Interfaces;
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
    private readonly IUserValidator _userValidator;
    private readonly IUserService _userService;
    private readonly string _adminUsername;
    private readonly string _adminEmail;
    private readonly string _adminPassword;

    public AuthController(
        ILogger<AuthController> logger,
        IUserValidator userValidator,
        IUserService userService
        )
    {
        _logger = logger;
        _userValidator = userValidator;
        _userService = userService;
        _adminUsername = "admin";
        _adminEmail = "admin@app.com";
        _adminPassword = "Admin@p1p2p3";
    }

    public IActionResult Login()
    {
        _logger.LogInformation("Landed to login page");

        if (HttpContext.User?.Identity.IsAuthenticated == true)
        {
            _logger.LogInformation("User already authenticated, redirecting");
            return RedirectToAction("Index", "Home");
        }

        return View(new LoginVm());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        try
        {
            _logger.LogInformation("Login initiated by {Email}", vm.Email);

            await _userService.LoginAsync(new UserDto(string.Empty, vm.Email, vm.Password));

            await ContextSignInAsync(vm);

            _logger.LogInformation("Login successful, redirecting");

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Login not successful");
            return View(new LoginVm { WarnMessage = ex.Message });
        }

        async Task ContextSignInAsync(LoginVm vm)
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
        }
    }

    public async Task<IActionResult> Logout()
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        _logger.LogInformation("Logout initiated by {email}", email);

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        _logger.LogInformation("Logout successful");

        return RedirectToAction(nameof(Login));
    }

    public async Task<IActionResult> Seed()
    {
        try
        {
            _logger.LogInformation("Seed initiated");

            using var txn = TxnScopeHelper.NewTxnScope;
            if (await _userValidator.IsUserNotExistsAsync(_adminEmail))
                await _userService.RegisterAsync(new UserDto(_adminUsername, _adminEmail, _adminPassword));

            _logger.LogInformation("Seeding successful, redirecting to login");

            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return RedirectToAction(nameof(Login), new LoginVm { WarnMessage = ex.Message });
        }
    }
}
