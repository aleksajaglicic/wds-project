using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using Trainer.Server.DTOs;
using Trainer.Server.Entities;
using Trainer.Server.Interfaces;

namespace Trainer.Server.Helpers
{
    public class DtoMapper
    {
        private readonly IPasswordHasher _hasher;

        public DtoMapper() { }

        public User DtoToUser(UserDto dto)
        {
            if(dto != null)
            {
                var user = new User
                {
                    Name = dto.Name,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PasswordHash = _hasher.HashPassword(dto.Password),
                    Role = "User",
                    Workouts = null
                };

                return user;
            }

            return null;
        }
    }
}
