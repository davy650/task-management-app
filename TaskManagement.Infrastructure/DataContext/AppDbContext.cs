namespace TaskManagement.Infrastructure.DataContext;

using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        // Task entity configuration
        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.HasOne(t => t.Assignee)
                  .WithMany(u => u.AssignedTasks)
                  .HasForeignKey(t => t.AssignedTo)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(t => t.Creator)
                  .WithMany(u => u.CreatedTasks)
                  .HasForeignKey(t => t.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}