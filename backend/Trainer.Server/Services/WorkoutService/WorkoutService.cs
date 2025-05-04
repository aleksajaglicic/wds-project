namespace Trainer.Server.Services.WorkoutService
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.OpenApi.Validations;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Trainer.Server.Data;
    using Trainer.Server.DTOs;
    using Trainer.Server.Entities;
    using Trainer.Server.Enums;
    using Trainer.Server.Interfaces;

    public class WorkoutService : IWorkoutService
    {
        #region Props
        private readonly IMongoCollection<Workout>? _workouts;
        private readonly IDtoMapper _mapper;
        private readonly IUserService _userService;
        #endregion

        #region Constructor
        public WorkoutService(MongoDbService mongoDbService, IDtoMapper dtoMapper, IUserService userService)
        {
            _workouts = mongoDbService.Database?.GetCollection<Workout>("workout");
            _mapper = dtoMapper;
            _userService = userService;
        }
        #endregion

        #region Methods
        //Create workout
        public async Task<bool> CreateWorkoutAsync(WorkoutDto? workoutDto)
        {
            if (workoutDto == null)
            {
                return false;
            }

            var workout = _mapper.ToWorkoutEntity(workoutDto);
            workout.Id = ObjectId.GenerateNewId();

            await _workouts.InsertOneAsync(workout);

            return await _userService.InsertWorkoutId(workout.Id, workout.UserId);
        }

        //Delete workout
        public async Task<bool> DeleteWorkoutAsync(string? id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return false;
            }

            var result = await _workouts.DeleteOneAsync(x => x.Id == objectId);
            return result.DeletedCount > 0;
        }

        //Get all workouts
        public async Task<IEnumerable<WorkoutDto>?> GetAll()
        {
            var workouts = await _workouts.Find(_ => true).ToListAsync();
            return workouts.Select(_mapper.ToWorkoutDto);
        }

        public async Task<WorkoutSummaryDto> GetWeeklySummaryAsync(string userId, string month, int week)
        {
            if (!ObjectId.TryParse(userId, out var objectId))
            {
                return null;
            }

            var monthNumber = int.Parse(month);

            var startOfMonth = new DateTime(DateTime.Now.Year, monthNumber, 1, 0, 0, 0, DateTimeKind.Utc);
            var endOfMonth = startOfMonth.AddMonths(1); 
            var endOfMonthUtc = new DateTime(endOfMonth.Year, endOfMonth.Month, endOfMonth.Day, 0, 0, 0, DateTimeKind.Utc);
            var filter = Builders<Workout>.Filter.And(
                Builders<Workout>.Filter.Eq(w => w.UserId, objectId),
                Builders<Workout>.Filter.Gte(w => w.WorkoutDate, startOfMonth),
                Builders<Workout>.Filter.Lt(w => w.WorkoutDate, endOfMonthUtc)
            );

            var workouts = await _workouts.Find(filter).ToListAsync();

            if (workouts == null || workouts.Count == 0)
            {
                return null;
            }

            int weekStartDay = (week - 1) * 7 + 1;
            int weekEndDay = week * 7;

            var weeklyWorkouts = workouts.Where(w =>
            {
                var day = w.WorkoutDate.Day;
                return day >= weekStartDay && day <= weekEndDay;
            }).ToList();

            if (!weeklyWorkouts.Any())
            {
                return null;
            }

            var totalDuration = TimeSpan.FromMinutes(weeklyWorkouts.Sum(w => w.Duration ?? 0));
            var workoutCount = weeklyWorkouts.Count;
            var avgDifficulty = weeklyWorkouts.Average(w => w.Difficulty ?? 0);
            var avgTiredness = weeklyWorkouts.Average(w => w.Tiredness ?? 0);

            return new WorkoutSummaryDto
            {
                WeekNumber = week,
                TotalDuration = totalDuration,
                WorkoutCount = workoutCount,
                AvgDifficulty = avgDifficulty,
                AvgTiredness = avgTiredness
            };
        }


        public static int GetWeekNumber(DateTime date)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.Calendar.GetWeekOfYear(
                date,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }


        //Paginate workouts
        public async Task<IEnumerable<WorkoutDto>?> GetWorkoutByPage(string id, int page, int numOfElements)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var workouts =  await _workouts
                .Find(x => x.UserId == objectId)
                .SortBy(o => o.WorkoutDate)
                .Skip(page == null ? 0 : (page - 1) * numOfElements)
                .Limit(numOfElements)
                .ToListAsync();

            return workouts.Select(_mapper.ToWorkoutDto).ToList();
        }

        //Update workout
        public async Task<WorkoutDto> UpdateWorkoutAsync(WorkoutDto? workoutDto)
        {
            var update = Builders<Workout>.Update
                .Set(x => x.WorkoutDate, DateTime.Parse(workoutDto.WorkoutDate))
                .Set(x => x.WorkoutType, Enum.Parse<WorkoutType>(workoutDto.WorkoutType))
                .Set(x => x.Tiredness, workoutDto.Tiredness)
                .Set(x => x.Difficulty, workoutDto.Difficulty)
                .Set(x => x.CaloriesSpent, workoutDto.CaloriesSpent)
                .Set(x => x.Duration, workoutDto.Duration)
                .Set(x => x.AdditionalNote, workoutDto.AdditionalNote);

            var filter = Builders<Workout>.Filter.Eq(x => x.Id, ObjectId.Parse(workoutDto.Id));

            await _workouts.UpdateOneAsync(filter, update);
            var workout = await _workouts.Find(filter).FirstOrDefaultAsync();
            return _mapper.ToWorkoutDto(workout);
        }
        #endregion
    }
}
