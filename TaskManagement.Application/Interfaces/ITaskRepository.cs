namespace TaskManagement.Application.Interfaces;

using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

public interface ITaskRepository
{
    Task<UserTask> GetTaskByIdAsync(Guid id);
    Task<IEnumerable<UserTask>> GetTasksAsync(TaskStatus? status, Guid? assigneeId);
    Task<UserTask> AddTaskAsync(UserTask task);
    Task<UserTask> UpdateTaskAsync(UserTask task);
    Task DeleteTaskAsync(UserTask task);
}