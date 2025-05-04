namespace Trainer.Server.Interfaces
{
    using Trainer.Server.DTOs;
    using Trainer.Server.Entities;

    public interface IDtoMapper
    {
        UserDto ToDto(User user);
        User ToEntity(UserDto dto);
        WorkoutDto ToWorkoutDto(Workout workout);
        Workout ToWorkoutEntity(WorkoutDto workoutDto);
    }
}
