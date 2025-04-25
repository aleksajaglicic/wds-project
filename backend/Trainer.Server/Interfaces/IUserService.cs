namespace Trainer.Server.Interfaces
{
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using Trainer.Server.Entities;

    public interface IUserService
    {
        Task<User?> GetUserById(ObjectId? _objectId);
        Task<IEnumerable<User>?> GetAll();
        Task<bool> CreateUserAsync(User? user);
        Task<bool> UpdateUserAsync(User? user);
        Task<bool> DeleteUserAsync(ObjectId? _objectId);
        Task<User?> GetUserByEmail(string email);
    }
}
