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
    public class UserServiceTests
    {
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
            .UseMySql("Server=localhost;Database=FitnessTrackerDb;User=root;",
          ServerVersion.AutoDetect("Server=localhost;Database=FitnessTrackerDb;User=root;"))



            _context = new AppDbContext(_options);
            _userService = new UserService(_context);
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
            var name = "Original User";
            var googleId = "google123";
            var avatarUrl = "https://example.com/original-avatar.jpg";

            var existingUser = new User
            {
                Email = email,
                Name = name,
                GoogleId = null,
                GoogleAvatarUrl = null,
                IsGoogleLinked = false
            };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var newGoogleId = "google456";
            var newAvatarUrl = "https://example.com/new-avatar.jpg";

            // Act
            var user = await _userService.RegisterOrUpdateGoogleUserAsync(email, name, newGoogleId, newAvatarUrl);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
            Assert.Equal(name, user.Name); // Name may remain unchanged
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
            _context.Users.Add(new User
            {
                Email = "user2@example.com",
                Name = "User2",
                GoogleId = googleId,
                IsGoogleLinked = true
            });
            await _context.SaveChangesAsync();

            var email = "user3@example.com";
            var name = "User3";

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() =>
                _userService.RegisterOrUpdateGoogleUserAsync(email, name, googleId, null));

        }
    }
}
