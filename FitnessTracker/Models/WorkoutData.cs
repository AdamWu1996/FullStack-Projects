namespace FitnessTracker.Models
{
    public class WorkoutData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime WorkoutDate { get; set; }
        public string? ExerciseName { get; set; }
        public int Duration { get; set; }
        public float CaloriesBurned { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User? User { get; set; }  // 外鍵連接 User 表
        public ICollection<SharedData>? SharedDatas { get; set; }
    }
}
