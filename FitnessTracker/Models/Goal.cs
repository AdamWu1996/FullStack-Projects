namespace FitnessTracker.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? GoalType { get; set; }
        public float TargetValue { get; set; }
        public float CurrentValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User? User { get; set; }
    }
}
