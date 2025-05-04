
namespace Trainer.Server.Interfaces
{
    using Trainer.Server.Entities;

    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
