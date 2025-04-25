using Trainer.Server.DTOs;
using Trainer.Server.Entities;

namespace Trainer.Server.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserDto dto);
        Task<string?> LoginAsync(UserLoginDto dto);
        string GenerateToken(User user);
    }
}
