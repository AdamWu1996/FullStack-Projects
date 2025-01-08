using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// 設定 Google 驗證
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // 設置默認的 SignInScheme
})
.AddCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.LogoutPath = "/auth/logout";
})
.AddGoogle(options =>
{
    options.ClientId = "202351897366-m7cg2q9pdj06s3cjcgjf49ohc9e9et4h.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.CallbackPath = "/oauth2callback"; // 配置與 GCP 重定向 URI 一致
    options.Scope.Add("email"); // 確保返回 email
    options.Scope.Add("profile"); // 確保返回用戶基本資料（如 name）
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
