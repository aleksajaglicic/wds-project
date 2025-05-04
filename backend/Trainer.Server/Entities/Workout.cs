namespace Trainer.Server.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Trainer.Server.Enums;

    public class Workout
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId? Id { get; set; }

        [BsonElement("user_id")]
        public ObjectId? UserId { get; set; }

        [BsonElement("workout_date")]
        public DateTime WorkoutDate { get; set; }

        [BsonElement("workout_type")]
        [BsonRequired]
        public WorkoutType WorkoutType { get; set; }

        [BsonElement("duration")]
        [BsonRequired]
        public int? Duration { get; set; }

        [BsonElement("tiredness")]
        [BsonRequired]
        public int? Tiredness { get; set; }

        [BsonElement("difficulty")]
        [BsonRequired]
        public int? Difficulty { get; set; }

        [BsonElement("calories_spent")]
        [BsonRequired]
        public int? CaloriesSpent { get; set; }

        [BsonElement("additional_note")]
        public string? AdditionalNote { get; set; }
    }
}
