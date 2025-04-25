namespace Trainer.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using Trainer.Server.DTOs;
    using Trainer.Server.Interfaces;
    using Trainer.Server.Services.AuthService;

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        public async Task<ActionResult<string>?> Login(UserLoginDto userLoginDto)
        {
            var token = await _authService.LoginAsync(userLoginDto);
            return token == null ? Unauthorized("Invalid credentials.") : Ok(new { Token = token });
        }

        public async Task<ActionResult<bool>?> Register(UserDto userDto)
        {
            var result = await _authService.RegisterAsync(userDto);
            if (!result) return BadRequest("User already exists.");
            return Ok("Registered successfully.");
        }
    }
}
