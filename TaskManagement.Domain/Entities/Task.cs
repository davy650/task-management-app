namespace TaskManagement.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Task
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public int Priority { get; set; }
    public guid? AssignedTo { get; set; }
    public guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // we will use this to get the actual users
    public User? Assignee { get; set; }
    public User Creator { get; set; }
}