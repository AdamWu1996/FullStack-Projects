using Microsoft.EntityFrameworkCore;
using FitnessTracker.Models;

namespace FitnessTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<WorkoutData> WorkoutDatas { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SharedData> SharedData { get; set; }
        public DbSet<JWTTokenBlacklist> JWTTokenBlacklists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //避免資料重複
            modelBuilder.Entity<User>()
                .HasIndex(u => u.GoogleId)
                .IsUnique();

            // 定義資料表的外鍵關聯等
            modelBuilder.Entity<WorkoutData>()
                .HasOne(w => w.User)
                .WithMany(u => u.WorkoutData)
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<Goal>()
                .HasOne(g => g.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<SharedData>()
                .HasOne(s => s.User)
                .WithMany(u => u.SharedDatas)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<SharedData>()
                .HasOne(s => s.WorkoutData)
                .WithMany(w => w.SharedDatas)
                .HasForeignKey(s => s.WorkoutDataId);
        }
    }
}
