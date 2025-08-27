using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TaskManagement.Application.Services;
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TaskDto> CreateTaskAsync(TaskDto taskDto, Guid creatorId)
    {
        var task = new UserTask
        {
            Id = taskDto.Id,
            Title = taskDto.Title,
            Description = taskDto.Description,
            Status = Enum.Parse<UserTaskStatus>(taskDto.Status, ignoreCase: true),
            Priority = taskDto.Priority,
            AssignedTo = taskDto.AssignedTo,
            CreatedBy = creatorId,
            CreatedAt = taskDto.CreatedAt,
            UpdatedAt = taskDto.UpdatedAt
        };

        var createdTask = await _taskRepository.AddTaskAsync(task);

        return new TaskDto
        {
            Id = createdTask.Id,
            Title = createdTask.Title,
            Description = createdTask.Description,
            Status = createdTask.Status.ToString(),
            Priority = createdTask.Priority,
            AssignedTo = createdTask.AssignedTo,
            CreatedBy = createdTask.CreatedBy,
            CreatedAt = createdTask.CreatedAt,
            UpdatedAt = createdTask.UpdatedAt
        };
    }

    public async Task DeleteTaskAsync(Guid id, Guid userId)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }

        if (task.CreatedBy != userId)
        {
            var userRole = GetUserRole();
            if (userRole != UserRole.ADMIN)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this task.");
            }
        }

        await _taskRepository.DeleteTaskAsync(task);
    }

    public async Task<TaskDto> GetTaskByIdAsync(Guid id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority,
            AssignedTo = task.AssignedTo,
            CreatedBy = task.CreatedBy,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }

    public async Task<IEnumerable<TaskDto>> GetTasksAsync(UserTaskStatus? status, Guid? assigneeId)
    {
        var tasks = await _taskRepository.GetTasksAsync(status, assigneeId);
        return tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            Priority = t.Priority,
            AssignedTo = t.AssignedTo,
            CreatedBy = t.CreatedBy,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        });
    }

    public async Task<TaskDto> UpdateTaskAsync(Guid id, TaskDto taskDto, Guid userId)
    {
        var existingTask = await _taskRepository.GetTaskByIdAsync(id);
        if (existingTask == null)
        {
            throw new KeyNotFoundException("Task not found.");
        }

        if (existingTask.CreatedBy != userId)
        {
            var userRole = GetUserRole();
            if (userRole != UserRole.ADMIN)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this task.");
            }
        }

        // Update the existing task with new data
        existingTask.Title = taskDto.Title;
        existingTask.Description = taskDto.Description;
        existingTask.Status = Enum.Parse<UserTaskStatus>(taskDto.Status, ignoreCase: true);
        existingTask.Priority = taskDto.Priority;
        existingTask.AssignedTo = taskDto.AssignedTo;
        existingTask.UpdatedAt = DateTime.UtcNow;

        var updatedTask = await _taskRepository.UpdateTaskAsync(existingTask);

        return new TaskDto
        {
            Id = updatedTask.Id,
            Title = updatedTask.Title,
            Description = updatedTask.Description,
            Status = updatedTask.Status.ToString(),
            Priority = updatedTask.Priority,
            AssignedTo = updatedTask.AssignedTo,
            CreatedBy = updatedTask.CreatedBy,
            CreatedAt = updatedTask.CreatedAt,
            UpdatedAt = updatedTask.UpdatedAt
        };
    }

    public async Task<IEnumerable<UserDto>> GetUsersForAssignmentAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        // return _mapper.Map<IEnumerable<UserDto>>(users);
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email, 
            Role = u.Role.ToString()
        });
    }

    private UserRole GetUserRole()
    {
        var userRoleClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        return (UserRole)Enum.Parse(typeof(UserRole), userRoleClaim);
    }
}
