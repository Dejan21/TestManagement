using TestManagement.Models;

namespace TestManagement.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskModel>> GetAll(string status = "all");
        Task<TaskModel?> GetById(int id);
        Task<int> Create(string title, string? description, byte priority, DateTime? dueDate);
        Task<bool> Update(int id, string title, string? description, byte priority, DateTime? dueDate, bool isCompleted);
        Task<bool> SetComplete(int id, bool value);
        Task<bool> Delete(int id);
    }
}
