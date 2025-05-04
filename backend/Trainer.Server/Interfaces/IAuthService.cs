namespace Trainer.Server.Interfaces
{
    using Trainer.Server.DTOs;
    
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserDto dto);
        Task<string?> LoginAsync(UserLoginDto dto);
    }
}
