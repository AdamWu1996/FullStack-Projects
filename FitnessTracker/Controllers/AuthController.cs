using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public class AuthController : Controller
    {
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

            // 正確提取 Email 和 Name
            var email = authenticateResult.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            var name = authenticateResult.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            // 確認提取的資訊
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Failed to retrieve email or name.");
                return BadRequest("Unable to retrieve user information.");
            }

            Console.WriteLine($"User Email: {email}");
            Console.WriteLine($"User Name: {name}");

            // 在這裡可以保存用戶資訊到資料庫
            // UserService.AddOrUpdateUser(email, name);

            // 成功後重定向到首頁或其他頁面
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
