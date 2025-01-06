using FitnessTracker.Models;

namespace FitnessTracker.Models
{
    public class SharedData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int WorkoutDataId { get; set; }
        public string? SharedPlatform { get; set; }
        public DateTime SharedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User? User { get; set; }
        public WorkoutData? WorkoutData { get; set; }
    }
}
