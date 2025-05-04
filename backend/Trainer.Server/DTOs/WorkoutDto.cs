namespace Trainer.Server.DTOs
{
    public class WorkoutDto
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? WorkoutDate { get; set; }
        public string? WorkoutType { get; set; } 
        public int? Duration { get; set; }
        public int? Tiredness { get; set; }
        public int? Difficulty { get; set; }
        public int? CaloriesSpent { get; set; }
        public string? AdditionalNote { get; set; }
    }
}
