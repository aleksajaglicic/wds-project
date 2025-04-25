namespace Trainer.Server.Services.AuthService
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Tokens;
    using Trainer.Server.DTOs;
    using Trainer.Server.Entities;
    using Trainer.Server.Interfaces;

    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _hasher;
        private readonly IConfiguration _configuration;

        public AuthService(IUserService userService, IConfiguration configuration, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _configuration = configuration;
            _hasher = passwordHasher;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string?> LoginAsync(UserLoginDto dto)
        {
            var user = await _userService.GetUserByEmail(dto.Email);
            if (user == null || !_hasher.VerifyPassword(user.PasswordHash, dto.Password))
                return null;

            return GenerateToken(user);
        }

        public async Task<bool> RegisterAsync(UserDto dto)
        {
            var existingUser = await _userService.GetUserByEmail(dto.Email);
            if (existingUser != null) return false;

            var user = new User
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = _hasher.HashPassword(dto.Password),
                Role = "User",
                Workouts = null
            };

            return await _userService.CreateUserAsync(user);
        }
    }
}
