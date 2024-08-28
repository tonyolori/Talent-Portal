using Domain.Entities;

namespace Infrastructure.Data;
public class TaskManager
{
    private readonly IRepository<ModuleTask> _taskRepository;

    public TaskManager(IRepository<ModuleTask> taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<ModuleTask>> GetAllTasks()
    {
        return await _taskRepository.GetAllAsync();
    }

    public async Task<ModuleTask?> GetTaskById(Guid taskId)
    {
        return await _taskRepository.GetByIdAsync(taskId);
    }

    public async Task CreateTask(ModuleTask task)
    {
        await _taskRepository.CreateAsync(task);
    }

    public async Task UpdateTask(ModuleTask task)
    {
        await _taskRepository.UpdateAsync(task);
    }

    public async Task DeleteTask(Guid taskId)
    {
        await _taskRepository.DeleteAsync(taskId);
    }

    public async Task<List<ModuleTask>> GetTasksByStatus(string status)
    {
        return await _taskRepository.GetByConditionAsync(t => t.Status == status);
    }

    public async Task<List<ModuleTask?>> GetTasksByCourseId(int courseId)
    {
        return await _taskRepository.GetByConditionAsync(t => t.CourseId == courseId);
    }
}
