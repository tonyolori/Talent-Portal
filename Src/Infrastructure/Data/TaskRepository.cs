using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure.Data;
public class TaskRepository : IRepository<ModuleTask>
{
    private readonly ApplicationDbContext _dbContext;

    public TaskRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ModuleTask>> GetAllAsync()
    {
        return await _dbContext.Tasks.ToListAsync();
    }

    public async Task<ModuleTask?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.TaskId == id);
    }

    public async Task CreateAsync(ModuleTask task)
    {
        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ModuleTask task)
    {
        _dbContext.Tasks.Update(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.TaskId == id);
        if (task != null)
        {
            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<ModuleTask>> GetByConditionAsync(Expression<Func<ModuleTask, bool>> predicate)
    {
        return await _dbContext.Tasks.Where(predicate).ToListAsync();
    }
}