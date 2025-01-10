// Services/UserService.cs
using FitnessTracker.Models;
using FitnessTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<User> RegisterOrUpdateGoogleUserAsync(string email, string name, string googleId, string avatarUrl)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                // 新增用戶
                user = new User
                {
                    Email = email,
                    Name = name,
                    GoogleId = googleId,
                    GoogleAvatarUrl = avatarUrl,
                    IsGoogleLinked = true
                };

                _context.Users.Add(user);
            }
            else
            {
                // 更新用戶
                if (!string.IsNullOrEmpty(googleId) && user.GoogleId != googleId)
                {
                    user.GoogleId = googleId;
                }

                if (!string.IsNullOrEmpty(avatarUrl))
                {
                    user.GoogleAvatarUrl = avatarUrl;
                }

                user.IsGoogleLinked = true;

                // 可選：決定是否覆蓋名稱
                user.Name = name;
            }

            await _context.SaveChangesAsync();

            return user;
        }

    }
}
