namespace Trainer.Server.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class User
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId? Id { get; private set; }

        [BsonElement("user_name")]
        [BsonRequired]
        public string? Name { get; set; }

        [BsonElement("user_lastName")]
        [BsonRequired]
        public string? LastName { get; set; }

        [BsonElement("email")]
        [BsonRequired]
        public string? Email { get; set; }

        [BsonElement("user_password")]
        [BsonRequired]
        public string? PasswordHash { get; private set; }

        [BsonElement("workouts")]
        public List<ObjectId>? Workouts { get; set; } = new();
    }
}
