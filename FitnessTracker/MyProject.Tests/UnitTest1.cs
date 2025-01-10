using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.Services;

namespace FitnessTracker.Tests
{
    public class UserServiceTests : IDisposable
    {
        public void Dispose()
        {
            _context.Database.ExecuteSqlRaw("DELETE FROM Users");
        }
        private readonly UserService _userService;
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;

        public UserServiceTests()
        {
            // 使用 InMemoryDatabase 模擬數據庫
            // _options = new DbContextOptionsBuilder<AppDbContext>()
            //     .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            //     .Options;

            // _context = new AppDbContext(_options);
            // _userService = new UserService(_context);

            // 使用真實的資料庫配置
            _options = new DbContextOptionsBuilder<AppDbContext>()
                        .UseMySql("Server=localhost;Database=FitnessTrackerDB;User=root;Password=u831i6j6;",
                                ServerVersion.AutoDetect("Server=localhost;Database=FitnessTrackerDB;User=root;Password=u831i6j6;"))
                        .Options;


            _context = new AppDbContext(_options);
            _userService = new UserService(_context);
            _context.Database.EnsureDeleted(); // 清理測試資料
            _context.Database.EnsureCreated(); // 初始化資料庫
        }

        [Fact]
        public async Task RegisterOrUpdateGoogleUserAsync_ShouldCreateNewUser_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "testuser@example.com";
            var name = "Test User";
            var googleId = "google123";
            var avatarUrl = "https://example.com/avatar.jpg";

            // Act
            var user = await _userService.RegisterOrUpdateGoogleUserAsync(email, name, googleId, avatarUrl);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
            Assert.Equal(name, user.Name);
            Assert.Equal(googleId, user.GoogleId);
            Assert.Equal(avatarUrl, user.GoogleAvatarUrl);
            Assert.True(user.IsGoogleLinked);
        }

        [Fact]
        public async Task RegisterOrUpdateGoogleUserAsync_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var email = "testuser@example.com";
            var originalName = "Original User";
            var newName = "Updated User";
            var googleId = "google123";

            var existingUser = new User
            {
                Email = email,
                Name = originalName,
                GoogleId = null, // 初始用戶無 GoogleId
                GoogleAvatarUrl = null,
                IsGoogleLinked = false
            };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var newGoogleId = "google123";
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            // Act
            var user = await _userService.RegisterOrUpdateGoogleUserAsync(email, newName, newGoogleId, newAvatarUrl);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
            Assert.Equal(newName, user.Name); // 預期名稱被更新
            Assert.Equal(newGoogleId, user.GoogleId);
            Assert.Equal(newAvatarUrl, user.GoogleAvatarUrl);
            Assert.True(user.IsGoogleLinked);
        }


        [Fact]
        public async Task RegisterOrUpdateGoogleUserAsync_ShouldThrowException_WhenGoogleIdIsDuplicated()
        {
            // Arrange
            var googleId = "duplicate-google-id";
            _context.Users.Add(new User
            {
                Email = "user1@example.com",
                Name = "User1",
                GoogleId = googleId,
                IsGoogleLinked = true
            });
            await _context.SaveChangesAsync();

            var email = "user2@example.com";
            var name = "User2";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DbUpdateException>(() =>
                _userService.RegisterOrUpdateGoogleUserAsync(email, name, googleId, null));

            Assert.Contains("Duplicate entry", exception.InnerException.Message);
        }

    }
}
