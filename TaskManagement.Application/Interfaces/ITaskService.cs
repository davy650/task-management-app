namespace TaskManagement.Application.Interfaces;

using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Enums;

public interface ITaskService
{
    Task<TaskDto> CreateTaskAsync(TaskDto taskDto, Guid creatorId);
    Task<TaskDto> UpdateTaskAsync(Guid id, TaskDto taskDto, Guid userId);
    Task DeleteTaskAsync(Guid id, Guid userId);
    Task<IEnumerable<TaskDto>> GetTasksAsync(UserTaskStatus? status, Guid? assigneeId);
    Task<TaskDto> GetTaskByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetUsersForAssignmentAsync();
}