namespace Trainer.Server.Helpers
{
    using MongoDB.Bson;
    using Trainer.Server.DTOs;
    using Trainer.Server.Entities;
    using Trainer.Server.Enums;
    using Trainer.Server.Interfaces;

    public class DtoMapper : IDtoMapper
    {
        #region Constructor
        public DtoMapper() { }
        #endregion

        #region UserMethods
        public User ToEntity(UserDto userDto)
        {
            if(userDto != null)
            {
                return new User
                {
                    Id = string.IsNullOrEmpty(userDto.Id) ? null : ObjectId.Parse(userDto.Id),
                    Name = userDto.Name,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    PasswordHash = userDto.Password,
                    Role = "User",
                    Workouts = new List<ObjectId>()
                };
            }

            return null;
        }

        public UserDto ToDto(User user)
        {
            if (user != null)
            {
                return new UserDto
                {
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = null
                };
            }

            return new UserDto { };
        }
        #endregion

        #region WorkoutMethods
        public WorkoutDto ToWorkoutDto(Workout workout)
        {
            if (workout != null)
            {
                return new WorkoutDto
                {
                    Id = workout.Id.ToString(),
                    UserId = workout.UserId.ToString(),
                    Duration = workout.Duration,
                    CaloriesSpent = workout.CaloriesSpent,
                    Tiredness = workout.Tiredness,
                    Difficulty = workout.Difficulty,
                    WorkoutDate = workout.WorkoutDate.ToString("yyyy-MM-ddTHH:mm"),
                    WorkoutType = workout.WorkoutType.ToString(),
                    AdditionalNote = workout.AdditionalNote
                };
            }

            return new WorkoutDto { };
        }

        public Workout ToWorkoutEntity(WorkoutDto workoutDto)
        {
            if (workoutDto != null)
            {
                return new Workout
                {
                    Id = string.IsNullOrEmpty(workoutDto.Id) ? null : ObjectId.Parse(workoutDto.Id),
                    UserId = ObjectId.Parse(workoutDto.UserId),
                    CaloriesSpent = workoutDto.CaloriesSpent,
                    Duration = workoutDto.Duration,
                    Tiredness = workoutDto.Tiredness,
                    Difficulty = workoutDto.Difficulty,
                    WorkoutDate = DateTime.Parse(workoutDto.WorkoutDate),
                    WorkoutType = Enum.Parse<WorkoutType>(workoutDto.WorkoutType),
                    AdditionalNote = workoutDto.AdditionalNote
                };
            }

            return new Workout { };
        }
        #endregion
    }
}
