namespace Trainer.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using Trainer.Server.DTOs;
    using Trainer.Server.Interfaces;
    using Trainer.Server.Services.AuthService;

    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Props
        private readonly IAuthService _authService;
        #endregion

        #region Constructor
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Login
        [HttpPost("login")]
        public async Task<ActionResult<string>?> Login([FromBody] UserLoginDto userLoginDto)
        {
            var token = await _authService.LoginAsync(userLoginDto);
            return token is not null ? Ok(new { Token = token }) : Unauthorized("Invalid credentials.");
        }
        #endregion

        #region Register
        [HttpPost("register")]
        public async Task<ActionResult<bool>?> Register([FromBody] UserDto userDto)
        {
            var result = await _authService.RegisterAsync(userDto);
            return result ? Ok("Registered successfully.") : BadRequest("User already exists.");
        }
        #endregion
    }
}
