using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SiteConstructor.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    public AuthController(IConfiguration config) => _config = config;

    public record LoginRequest(string Username, string Password);

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var user = _config["Admin:Username"];
        var pass = _config["Admin:Password"];

        if (req.Username != user || req.Password != pass)
            return Unauthorized(new { error = "Неверный логин или пароль" });

        var claims = new List<Claim> { new(ClaimTypes.Name, req.Username) };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return Ok(new { username = req.Username });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    [HttpGet("me")]
    public IActionResult Me()
        => User.Identity?.IsAuthenticated == true
            ? Ok(new { username = User.Identity!.Name })
            : Unauthorized();
}