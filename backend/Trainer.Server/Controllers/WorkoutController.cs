namespace Trainer.Server.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Trainer.Server.DTOs;
    using Trainer.Server.Interfaces;

    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        #region Props
        private readonly IWorkoutService _workoutService;
        #endregion

        #region Constructor
        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }
        #endregion

        #region CreateMethod
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> Create([FromBody] WorkoutDto workoutDto)
        {
            var result = await _workoutService.CreateWorkoutAsync(workoutDto);
            return result ? Ok(result) : BadRequest();
        }
        #endregion

        #region GetMethods
        //Get
        [HttpGet("{id}/{pageNumber}/{numOfElements}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> GetByPage(string id, int pageNumber, int numOfElements)
        {
            var result = await _workoutService.GetWorkoutByPage(id, pageNumber, numOfElements);
            return result is not null ? Ok(result) : BadRequest();
        }

        [HttpGet("summary/{userId}/{month}/{week}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> GetWeeklySummary(string userId, string month, int week)
        {
            var result = await _workoutService.GetWeeklySummaryAsync(userId, month, week);
            return result != null ? Ok(result) : BadRequest("Summary not found.");
        }
        #endregion

        #region UpdateMethod
        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> Update([FromBody] WorkoutDto workoutDto)
        {
            var result = await _workoutService.UpdateWorkoutAsync(workoutDto);
            return result is not null ? Ok(workoutDto) : BadRequest();
        }
        #endregion

        #region DeleteMethod
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _workoutService.DeleteWorkoutAsync(id);
            return result ? Ok("Successfully deleted.") : BadRequest();
        }
        #endregion
    }
}
