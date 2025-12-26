using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TestManagement.Models;
using TestManagement.Repositories;

namespace TaskManagement.Repositories

{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _cs;

        public TaskRepository(IConfiguration config)
        {
            _cs = config.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string: ConnectionStrings:Default");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_cs);

        public async Task<IEnumerable<TaskModel>> GetAll(string status = "all")
        {
            var where = status.ToLowerInvariant() switch
            {
                "active" => "WHERE IsCompleted = 0",
                "completed" => "WHERE IsCompleted = 1",
                _ => ""
            };

            var sql = $"""
            SELECT *
            FROM dbo.Tasks
            {where}
            ORDER BY CreatedAt DESC
        """;

            using var conn = CreateConnection();
            return await conn.QueryAsync<TaskModel>(sql);
        }

        public async Task<TaskModel?> GetById(int id)
        {
            const string sql = "SELECT * FROM dbo.Tasks WHERE Id = @Id";
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<TaskModel>(sql, new { Id = id });
        }

        public async Task<int> Create(string title, string? description, byte priority, DateTime? dueDate)
        {
            const string sql = """
            INSERT INTO dbo.Tasks (Title, Description, IsCompleted, Priority, DueDate, CreatedAt)
            VALUES (@Title, @Description, 0, @Priority, @DueDate, SYSUTCDATETIME());

            SELECT CAST(SCOPE_IDENTITY() AS int);
        """;

            using var conn = CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, new
            {
                Title = title,
                Description = description,
                Priority = priority,
                DueDate = dueDate
            });
        }

        public async Task<bool> Update(int id, string title, string? description, byte priority, DateTime? dueDate, bool isCompleted)
        {
            const string sql = """
            UPDATE dbo.Tasks
            SET Title = @Title,
                Description = @Description,
                Priority = @Priority,
                DueDate = @DueDate,
                IsCompleted = @IsCompleted,
                UpdatedAt = SYSUTCDATETIME()
            WHERE Id = @Id
        """;

            using var conn = CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new
            {
                Id = id,
                Title = title,
                Description = description,
                Priority = priority,
                DueDate = dueDate,
                IsCompleted = isCompleted
            });

            return rows > 0;
        }

        public async Task<bool> SetComplete(int id, bool value)
        {
            const string sql = """
            UPDATE dbo.Tasks
            SET IsCompleted = @Value,
                UpdatedAt = SYSUTCDATETIME()
            WHERE Id = @Id
            """;

            using var conn = CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { Id = id, Value = value });
            return rows > 0;
        }

        public async Task<bool> Delete(int id)
        {
            const string sql = "DELETE FROM dbo.Tasks WHERE Id = @Id";
            using var conn = CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
