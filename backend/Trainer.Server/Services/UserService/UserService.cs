namespace Trainer.Server.Services.UserService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Trainer.Server.Data;
    using Trainer.Server.Entities;
    using Trainer.Server.Interfaces;

    public class UserService : IUserService
    {
        #region Props
        private readonly IMongoCollection<User>? _users;
        #endregion

        #region Constructor
        public UserService(MongoDbService mongoDbService)
        {
            _users = mongoDbService.Database?.GetCollection<User>("user");
        }
        #endregion

        #region Methods
        public async Task<bool> CreateUserAsync(User? user)
        {
            try
            {
                if (user != null)
                {
                    await _users.InsertOneAsync(user);
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(ObjectId? _objectId)
        {
            DeleteResult result = null;

            try
            {
                var filter = Builders<User>.Filter.Eq(x => x.Id, _objectId);
                result =  await _users.DeleteOneAsync(filter);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result.DeletedCount == 1;
        }

        public async Task<IEnumerable<User>?> GetAll()
        {
            IEnumerable<User> result = null;

            try
            {
                result = await _users.Find(_ => true).ToListAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result is not null ? result : Enumerable.Empty<User>();
        }

        public Task<User?> GetUserById(ObjectId? _objectId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(User? user)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
