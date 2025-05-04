namespace Trainer.Server.Services.AuthService
{
    using System.Threading.Tasks;
    using Trainer.Server.DTOs;
    using Trainer.Server.Interfaces;

    public class AuthService : IAuthService
    {
        #region Props
        private readonly IUserService _userService;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenService _jwtTokenService;
        #endregion

        #region Constructor
        public AuthService(IUserService userService, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _hasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }
        #endregion

        #region Methods
        public async Task<string?> LoginAsync(UserLoginDto dto)
        {
            var user = await _userService.GetUserByEmail(dto.Email);
            if (user == null || !_hasher.VerifyPassword(user.PasswordHash, dto.Password))
                return null;

            return _jwtTokenService.GenerateToken(user);
        }

        public async Task<bool> RegisterAsync(UserDto dto)
        {
            var existingUser = await _userService.GetUserByEmail(dto.Email);
            if (existingUser != null) return false;

            return await _userService.CreateUserAsync(dto);
        }
        #endregion
    }
}
