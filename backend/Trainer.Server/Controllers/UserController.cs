namespace Trainer.Server.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Trainer.Server.DTOs;
    using Trainer.Server.Interfaces;

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        #region Props
        private readonly IUserService _userService;
        #endregion

        #region Constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Get
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var result = await _userService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetById([FromRoute] string id)
        {
            var result = await _userService.GetUserById(id);
            return result != null ? Ok(result) : NotFound();
        }
        #endregion

        #region Update
        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserDto>> Update([FromBody] UserDto user)
        {
            var result = await _userService.UpdateUserAsync(user);
            return result != null ? Ok(result) : BadRequest();
        }
        #endregion

        #region Delete
        [HttpPut("{userId}/{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> RemoveUserWorkout([FromRoute] string userId, string id)
        {
            var result = await _userService.RemoveWorkoutId(id, userId);
            return result ? Ok() : BadRequest();
        }
        #endregion
    }
}
