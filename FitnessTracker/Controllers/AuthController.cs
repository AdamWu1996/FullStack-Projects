using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Services;

namespace FitnessTracker.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("auth/login")]
        public IActionResult Login()
        {
            Console.WriteLine("Initiating Google Login...");
            var redirectUrl = Url.Action("GoogleCallback", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("auth/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            Console.WriteLine("Handling Google Callback...");
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                Console.WriteLine($"Authentication failed: {authenticateResult.Failure?.Message}");
                return BadRequest($"Authentication failed: {authenticateResult.Failure?.Message}");
            }

            Console.WriteLine("Google Authentication Succeeded!");

            var email = authenticateResult.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            var name = authenticateResult.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            var googleId = authenticateResult.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var avatarUrl = authenticateResult.Principal.FindFirst("urn:google:picture")?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(googleId))
            {
                Console.WriteLine("Failed to retrieve user information.");
                return BadRequest("Unable to retrieve user information.");
            }

            Console.WriteLine($"User Email: {email}");
            Console.WriteLine($"User Name: {name}");
            Console.WriteLine($"Google ID: {googleId}");
            Console.WriteLine($"Avatar URL: {avatarUrl}");

            // 更新用戶信息
            var user = await _userService.RegisterOrUpdateGoogleUserAsync(email, name, googleId, avatarUrl);

            if (user == null)
            {
                return BadRequest("Failed to register or update user.");
            }

            // TODO: 為用戶生成 JWT 或處理應用的 Session 管理

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("auth/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
