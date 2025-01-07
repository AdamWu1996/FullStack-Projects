using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class WorkoutController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWorkoutService _workoutService;

    public WorkoutController(AppDbContext context, IWorkoutService workoutService)
    {
        _context = context;
        _workoutService = workoutService;
    }

    [HttpPost]
    public async Task<IActionResult> AddWorkoutData([FromBody] WorkoutData data)
    {
        if (ModelState.IsValid)
        {
            _context.WorkoutDatas.Add(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }
        return BadRequest(ModelState);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllWorkoutData()
    {
        var data = await _context.WorkoutDatas.ToListAsync();
        return Ok(data);
    }
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetWorkoutDataByUser(int userId)
    {
        var data = await _context.WorkoutDatas.Where(w => w.UserId == userId).ToListAsync();
        return Ok(data);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkoutData(int id, [FromBody] WorkoutData data)
    {
        var existingData = await _context.WorkoutDatas.FindAsync(id);
        if (existingData == null) return NotFound();

        existingData.ExerciseName = data.ExerciseName;
        existingData.Weight = data.Weight;
        existingData.Repetitions = data.Repetitions;
        existingData.WorkoutDate = data.WorkoutDate;

        await _context.SaveChangesAsync();
        return Ok(existingData);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkoutData(int id)
    {
        var data = await _context.WorkoutDatas.FindAsync(id);
        if (data == null) return NotFound();

        _context.WorkoutDatas.Remove(data);
        await _context.SaveChangesAsync();
        return Ok();
    }

}
