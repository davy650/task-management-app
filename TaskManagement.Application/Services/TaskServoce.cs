namespace TaskManagement.Application.Services;

using AutoMapper;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TaskDto> CreateTaskAsync(TaskDto taskDto, Guid creatorId)
    {
        var task = _mapper.Map<UserTask>(taskDto);
        task.CreatedBy = creatorId;
        
        var createdTask = await _taskRepository.AddTaskAsync(task);
        return _mapper.Map<TaskDto>(createdTask);
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
        return _mapper.Map<TaskDto>(task);
    }

    public async Task<IEnumerable<TaskDto>> GetTasksAsync(UserTaskStatus? status, Guid? assigneeId)
    {
        var tasks = await _taskRepository.GetTasksAsync(status, assigneeId);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
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
        _mapper.Map(taskDto, existingTask);
        existingTask.UpdatedAt = DateTime.UtcNow;

        var updatedTask = await _taskRepository.UpdateTaskAsync(existingTask);
        return _mapper.Map<TaskDto>(updatedTask);
    }

    public async Task<IEnumerable<UserDto>> GetUsersForAssignmentAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    private UserRole GetUserRole()
    {
        var userRoleClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        return (UserRole)Enum.Parse(typeof(UserRole), userRoleClaim);
    }
}
