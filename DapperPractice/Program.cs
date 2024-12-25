using System.Data;
using Microsoft.Data.Sqlite; // 使用 SQLite 的命名空間
using Dapper;

class Program
{
    static void Main(string[] args)
    {
        // SQLite 資料庫的連接字串
        string connectionString = "Data Source=Promotions.db;";

        // 使用 SQLite 的連接
        using (IDbConnection db = new SqliteConnection(connectionString))
        {
            // 如果資料庫還沒有建立，可以初始化表
            InitializeDatabase(db);

            // 查詢資料
            var users = db.Query<User>("SELECT * FROM Users");
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Age: {user.Age}");
            }
        }
    }

    static void InitializeDatabase(IDbConnection db)
    {
        // 建立表（如果尚未存在）
        string createTableQuery = @"
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Age INTEGER NOT NULL
        );";
        db.Execute(createTableQuery);

        // 插入一些範例資料
        string insertDataQuery = @"
        INSERT INTO Users (Name, Age) VALUES ('Alice', 25), ('Bob', 30)
        ON CONFLICT DO NOTHING;"; // 避免重複插入
        db.Execute(insertDataQuery);
    }
}

// 使用者類別
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
