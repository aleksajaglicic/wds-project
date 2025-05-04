namespace Trainer.Server.Interfaces
{
    using MongoDB.Bson;
    using Trainer.Server.DTOs;
    using Trainer.Server.Entities;

    public interface IWorkoutService
    {
        Task<IEnumerable<WorkoutDto>?> GetWorkoutByPage(string id, int page, int numOfElements);
        Task<IEnumerable<WorkoutDto>?> GetAll();
        Task<bool> CreateWorkoutAsync(WorkoutDto? workoutDto);
        Task<WorkoutDto> UpdateWorkoutAsync(WorkoutDto? workoutDto);
        Task<bool> DeleteWorkoutAsync(string? id);
        Task<WorkoutSummaryDto> GetWeeklySummaryAsync(string userId, string month, int week);

    }
}
