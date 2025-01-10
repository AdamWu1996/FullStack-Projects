using FitnessTracker.Models;

// Services/IUserService.cs
namespace FitnessTracker.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> RegisterOrUpdateGoogleUserAsync(string email, string name, string googleId, string avatarUrl);
    }
}
