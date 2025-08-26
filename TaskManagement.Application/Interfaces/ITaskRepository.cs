namespace TaskManagement.Application.Interfaces;

using TaskManagement.Domain.Entities;

public interface ITaskRepository
{
    Task<Task> GetTaskByIdAsync(guid id);
    Task<IEnumerable<Task>> GetTasksAsync(TaskStatus? status, guid? assigneeId);
    Task<Task> AddTaskAsync(Task task);
    Task<Task> UpdateTaskAsync(Task task);
    Task DeleteTaskAsync(Task task);
}