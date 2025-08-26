namespace TaskManagement.Application.Interfaces;

using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Enums;

public interface ITaskService
{
    Task<TaskDto> CreateTaskAsync(TaskDto taskDto, guid creatorId);
    Task<TaskDto> UpdateTaskAsync(int id, TaskDto taskDto, guid userId);
    Task DeleteTaskAsync(guid id, guid userId);
    Task<IEnumerable<TaskDto>> GetTasksAsync(TaskStatus? status, guid? assigneeId);
    Task<TaskDto> GetTaskByIdAsync(guid id);
    Task<IEnumerable<UserDto>> GetUsersForAssignmentAsync();
}