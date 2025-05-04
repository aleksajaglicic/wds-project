namespace Trainer.Server.DTOs
{
    public class WorkoutSummaryDto
    {
        public int WeekNumber { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public int WorkoutCount { get; set; }
        public double AvgDifficulty { get; set; }
        public double AvgTiredness { get; set; }
    }
}
