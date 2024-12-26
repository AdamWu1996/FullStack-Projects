using System;
using Dapper;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        using var connection = new SqliteConnection("Data Source=:memory:;");
        connection.Open();

        // 創建表格
        connection.Execute(@"
            CREATE TABLE Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
            );

            CREATE TABLE Posts (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Content TEXT NOT NULL,
                OwnerId INTEGER NOT NULL,
                FOREIGN KEY (OwnerId) REFERENCES Users(Id)
            );
        ");

        Console.WriteLine("資料庫初始化完成！");

        var sqlPredicates = new List<string>();
        var queryParams = new DynamicParameters();

        sqlPredicates.Add("Name = @Name");
        queryParams.Add("Name", "Alice");

        string sql = "SELECT * FROM Users WHERE " + string.Join(" AND ", sqlPredicates);
        var filteredUsers = connection.Query(sql, queryParams);
    }
}
