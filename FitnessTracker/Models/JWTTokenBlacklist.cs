namespace FitnessTracker.Models
{
    public class JWTTokenBlacklist
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public DateTime BlacklistedAt { get; set; }
    }
}
