using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Enums;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] UserTaskStatus? status, [FromQuery] Guid? assignee)
    {
        var tasks = await _taskService.GetTasksAsync(status, assignee);
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
    {
        var creatorId = GetUserId();
        var task = await _taskService.CreateTaskAsync(taskDto, Guid.Parse(creatorId));
        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskDto taskDto)
    {
        var userId = GetUserId();
        var task = await _taskService.UpdateTaskAsync(id, taskDto, Guid.Parse(userId));
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var userId = GetUserId();
        await _taskService.DeleteTaskAsync(id, Guid.Parse(userId));
        return NoContent();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        return Ok(task);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsersForAssignment()
    {
        var users = await _taskService.GetUsersForAssignmentAsync();
        return Ok(users);
    }

    private string GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userIdClaim;
    }
}