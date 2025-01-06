using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Data;
using FitnessTracker.Models;

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

    // GET api/workout
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkoutData>>> GetWorkouts()
    {
        var workouts = await _context.Workouts.ToListAsync();
        return Ok(workouts);
    }

    // GET api/workout/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutData>> GetWorkout(int id)
    {
        var workout = await _context.Workouts.FindAsync(id);

        if (workout == null)
        {
            return NotFound();
        }

        return workout;
    }

    // POST api/workout
    [HttpPost]
    public async Task<ActionResult<WorkoutData>> PostWorkout(WorkoutData workout)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWorkout), new { id = workout.Id }, workout);
    }

    // PUT api/workout/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutWorkout(int id, WorkoutData workout)
    {
        if (id != workout.Id)
        {
            return BadRequest();
        }

        _context.Entry(workout).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE api/workout/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkout(int id)
    {
        var workout = await _context.Workouts.FindAsync(id);
        if (workout == null)
        {
            return NotFound();
        }

        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
