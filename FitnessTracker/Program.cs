using FitnessTracker.Services;
using FitnessTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 設定資料庫連線字串
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// 註冊 UserService 和 IUserService
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();

// 註冊 API 控制器服務
builder.Services.AddControllers();

// 註冊 Swagger 服務
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FitnessTracker API",
        Version = "v1",
        Description = "API for managing fitness tracking data"
    });
});

var app = builder.Build();

// 配置中介軟體
app.UseSwagger();  // 啟用 Swagger 生成器
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FitnessTracker API v1");  // 設定 Swagger UI 的接口文檔路徑
    options.RoutePrefix = "swagger";  // 設定 Swagger 頁面的 URL 路徑
});

app.UseAuthorization();
app.MapControllers();

app.Run();
