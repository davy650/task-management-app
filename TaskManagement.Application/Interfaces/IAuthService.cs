namespace TaskManagement.Application.Interfaces;

using TaskManagement.Application.DTOs;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto request);
    Task<LoginResponse> LoginAsync(LoginDto request);
}