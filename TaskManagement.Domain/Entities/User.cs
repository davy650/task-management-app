namespace TaskManagement.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

public class User
{
    public Guid Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.USER;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // we will need this model to be able to return assigned and created tasks per user 
    public ICollection<UserTask> AssignedTasks { get; set; }
    public ICollection<UserTask> CreatedTasks { get; set; }
}