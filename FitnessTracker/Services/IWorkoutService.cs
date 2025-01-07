using FitnessTracker.Models;

public interface IWorkoutService
{
    Task<IEnumerable<WorkoutData>> GetWorkoutsAsync();
    Task<WorkoutData> GetWorkoutByIdAsync(int id);
    Task AddWorkoutAsync(WorkoutData workout);
    Task UpdateWorkoutAsync(WorkoutData workout);
    Task DeleteWorkoutAsync(int id);
}
