namespace Trainer.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Trainer.Server.Data;
    using Trainer.Server.Entities;
    using Trainer.Server.Interfaces;
    using Trainer.Server.Services.UserService;

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User>? _customers;
        private readonly IUserService _userService;
        public UserController(MongoDbService mongoDbService, UserService userService)
        {
            _customers = mongoDbService.Database?.GetCollection<User>("user");
            _userService = userService;
        }

        //Get database
        [HttpGet]
        public async Task<ActionResult<User?>> Get()
        {
            var result = await _userService.GetAll();
            return result is not null ? Ok(result) : NotFound();
        }

        //Get user by id
        [HttpGet("{id}")]
        public async Task<ActionResult<User?>> GetById(ObjectId id)
        {
            var result = await _userService.GetUserById(id);
            return result is not null ? Ok(result) : NotFound();
        }

        //Create user
        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            var result = await _userService.CreateUserAsync(user);
            return result ? Ok() : BadRequest();
        }
    }
}
