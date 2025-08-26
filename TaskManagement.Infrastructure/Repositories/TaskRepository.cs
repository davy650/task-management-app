namespace TaskManagement.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.DataContext;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserTask> AddTaskAsync(UserTask task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task DeleteTaskAsync(UserTask task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
    
    public async Task<UserTask> GetTaskByIdAsync(Guid id)
    {
        return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<UserTask>> GetTasksAsync(UserTaskStatus? status, Guid? assigneeId)
    {
        IQueryable<UserTask> query = _context.Tasks;

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (assigneeId.HasValue)
        {
            query = query.Where(t => t.AssignedTo == assigneeId.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<UserTask> UpdateTaskAsync(UserTask task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return task;
    }
}
