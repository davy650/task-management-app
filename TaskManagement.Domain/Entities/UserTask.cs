namespace TaskManagement.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

public class UserTask
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.TODO;
    public int Priority { get; set; }
    public Guid? AssignedTo { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // we will use this to get the actual users
    public User? Assignee { get; set; }
    public User Creator { get; set; }
}