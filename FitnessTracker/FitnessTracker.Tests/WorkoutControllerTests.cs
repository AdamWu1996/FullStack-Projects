using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FitnessTracker.Data;
using FitnessTracker.Models;
using System;

namespace FitnessTracker.Tests
{
    [TestClass]
    public class WorkoutDataTests
    {
        private DbContextOptions<AppDbContext> _options;

        [TestInitialize]
        public void Setup()
        {
            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureDeleted();  // 清空資料庫
                context.Database.EnsureCreated();  // 重新創建資料庫
            }
        }

        public WorkoutDataTests()
        {
            // 設定 InMemoryDatabase
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
        }

        // Create Test
        [TestMethod]
        public void Test_WorkoutData_Creation()
        {
            using (var context = new AppDbContext(_options))
            {
                var WorkoutData = new WorkoutData
                {
                    Id = 1,
                    UserId = 123,
                    WorkoutDate = DateTime.Now,
                    ExerciseName = "Push-up",
                    Sets = 3,
                    Duration = 15,
                    Repetitions = 12,
                    Weight = 75f,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                context.WorkoutDatas.Add(WorkoutData);
                context.SaveChanges();

                // 確認資料是否正確儲存
                var savedData = context.WorkoutDatas.Find(1);
                Assert.IsNotNull(savedData);
                Assert.AreEqual(1, savedData.Id);
                Assert.AreEqual("Push-up", savedData.ExerciseName);
            }
        }

        // Read Test
        [TestMethod]
        public void Test_WorkoutData_Read()
        {
            using (var context = new AppDbContext(_options))
            {
                var WorkoutData = new WorkoutData
                {
                    Id = 1,
                    UserId = 123,
                    WorkoutDate = DateTime.Now,
                    ExerciseName = "Push-up",
                    Sets = 3,
                    Duration = 15,
                    Repetitions = 12,
                    Weight = 75f,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                context.WorkoutDatas.Add(WorkoutData);
                context.SaveChanges();

                var savedData = context.WorkoutDatas.Find(1);
                Assert.AreEqual(1, savedData.Id);
                Assert.AreEqual("Push-up", savedData.ExerciseName);
            }
        }

        // Update Test
        [TestMethod]
        public void Test_WorkoutData_Update()
        {
            using (var context = new AppDbContext(_options))
            {
                var WorkoutData = new WorkoutData
                {
                    Id = 1,
                    UserId = 123,
                    WorkoutDate = DateTime.Now,
                    ExerciseName = "Push-up",
                    Sets = 3,
                    Duration = 15,
                    Repetitions = 12,
                    Weight = 75f,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                context.WorkoutDatas.Add(WorkoutData);
                context.SaveChanges();

                // 更新資料
                WorkoutData.ExerciseName = "Pull-up";
                WorkoutData.Sets = 4;
                WorkoutData.Repetitions = 15;
                context.SaveChanges();

                var updatedData = context.WorkoutDatas.Find(1);
                Assert.AreEqual("Pull-up", updatedData.ExerciseName);
                Assert.AreEqual(4, updatedData.Sets);
                Assert.AreEqual(15, updatedData.Repetitions);
            }
        }

        // Delete Test
        [TestMethod]
        public void Test_WorkoutData_Delete()
        {
            using (var context = new AppDbContext(_options))
            {
                var WorkoutData = new WorkoutData
                {
                    Id = 1,
                    UserId = 123,
                    WorkoutDate = DateTime.Now,
                    ExerciseName = "Push-up",
                    Sets = 3,
                    Duration = 15,
                    Repetitions = 12,
                    Weight = 75f,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                context.WorkoutDatas.Add(WorkoutData);
                context.SaveChanges();

                context.WorkoutDatas.Remove(WorkoutData);
                context.SaveChanges();

                var deletedData = context.WorkoutDatas.Find(1);
                Assert.IsNull(deletedData);
            }
        }
    }
}
