namespace Trainer.Server.Interfaces
{
    using MongoDB.Bson;
    using Trainer.Server.DTOs;
    using Trainer.Server.Entities;

    public interface IUserService
    {
        Task<UserDto?> GetUserById(string? _objectId);
        Task<IEnumerable<UserDto>?> GetAll();
        Task<bool> CreateUserAsync(UserDto? user);
        Task<UserDto?> UpdateUserAsync(UserDto? user);
        Task<User?> GetUserByEmail(string email);
        Task<bool> InsertWorkoutId(ObjectId? workoutId, ObjectId? userId);
        Task<bool> RemoveWorkoutId(string? workoutId, string? userId);
    }
}
