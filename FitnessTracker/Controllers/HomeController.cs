using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            // 設定正確的 Content-Type 和字符編碼
            return Content("<h1>歡迎來到 FitnessTracker</h1><a href='/auth/login'>使用 Google 登入</a>", "text/html; charset=utf-8");
        }
    }
}
