using FitnessTracker.Models;

namespace FitnessTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // 新增身高、體重、年齡
        public float Height { get; set; }  // 身高，使用浮點數儲存
        public float Weight { get; set; }  // 體重，使用浮點數儲存
        public int Age { get; set; }      // 年齡，使用整數儲存
                                          // Google OAuth 整合相關欄位
        public string? GoogleId { get; set; }       // Google 帳號唯一 ID
        public bool IsGoogleLinked { get; set; }   // 是否連結 Google 帳號
        public string? GoogleAvatarUrl { get; set; } // Google 提供的用戶頭像 URL

        public ICollection<WorkoutData>? WorkoutData { get; set; }

        public ICollection<Notification>? Notifications { get; set; }

        public ICollection<SharedData>? SharedDatas { get; set; }

        public ICollection<Goal>? Goals { get; set; } // Added Goals collection

    }
}
