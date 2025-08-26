namespace TaskManagement.Application.Interfaces;

using TaskManagement.Domain.Entities;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> GetUserByIdAsync(guid id);
    Task<User> AddUserAsync(User user);
    Task<IEnumerable<User>> GetAllUsersAsync();
}