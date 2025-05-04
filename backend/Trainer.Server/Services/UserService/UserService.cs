namespace Trainer.Server.Services.UserService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Trainer.Server.Data;
    using Trainer.Server.DTOs;
    using Trainer.Server.Entities;
    using Trainer.Server.Interfaces;

    public class UserService : IUserService
    {
        #region Props
        private readonly IMongoCollection<User>? _users;
        private readonly IDtoMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        #endregion

        #region Constructor
        public UserService(MongoDbService mongoDbService, IDtoMapper dtoMapper, IPasswordHasher passwordHasher)
        {
            _users = mongoDbService.Database?.GetCollection<User>("user");
            _mapper = dtoMapper;
            _passwordHasher = passwordHasher;
        }
        #endregion

        #region Methods
        public async Task<bool> CreateUserAsync(UserDto? userDto)
        {
            if (userDto == null) return false;

            var user = _mapper.ToEntity(userDto);
            user.Id = ObjectId.GenerateNewId();
            user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);

            await _users.InsertOneAsync(user);
            return true;
        }

        public async Task<IEnumerable<UserDto>?> GetAll()
        {
            var users = await _users.Find(_ => true).ToListAsync();
            return users.Select(_mapper.ToDto);
        }

        public async Task<UserDto?> GetUserById(string? id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var user = await _users.Find(u => u.Id == objectId).FirstOrDefaultAsync();
            return user == null ? null : _mapper.ToDto(user);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
            return user is not null ? user : null;
        }

        public async Task<UserDto?> UpdateUserAsync(UserDto? userDto)
        {
            var update = Builders<User>.Update
                .Set(u => u.Name, userDto.Name)
                .Set(u => u.LastName, userDto.LastName)
                .Set(u => u.Email, userDto.Email);

            await _users.UpdateOneAsync(u => u.Email == userDto.Email, update);
            var updatedUser = await _users.Find(u => u.Email == userDto.Email).FirstOrDefaultAsync();

            return updatedUser is not null ? _mapper.ToDto(updatedUser) : null;
        }

        public async Task<bool> InsertWorkoutId(ObjectId? workoutId, ObjectId? userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update
                .Push<ObjectId>("workouts", (ObjectId)workoutId);

            var result = await _users.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> RemoveWorkoutId(string? workoutId, string? userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, ObjectId.Parse(userId));
            var update = Builders<User>.Update
                .Pull<ObjectId>("workouts", ObjectId.Parse(workoutId));

            var result = await _users.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
    #endregion
}
