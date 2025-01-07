using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

public class WorkoutService : IWorkoutService
{
    private readonly AppDbContext _context;

    public WorkoutService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkoutData>> GetWorkoutsAsync()
    {
        return await _context.WorkoutDatas.ToListAsync();
    }

    public async Task<WorkoutData> GetWorkoutByIdAsync(int id)
    {
        return await _context.WorkoutDatas.FindAsync(id);
    }

    public async Task AddWorkoutAsync(WorkoutData workout)
    {
        _context.WorkoutDatas.Add(workout);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateWorkoutAsync(WorkoutData workout)
    {
        _context.Entry(workout).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteWorkoutAsync(int id)
    {
        var workout = await _context.WorkoutDatas.FindAsync(id);
        if (workout != null)
        {
            _context.WorkoutDatas.Remove(workout);
            await _context.SaveChangesAsync();
        }
    }
}
