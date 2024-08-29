using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enum;

namespace Application.Interfaces;


public interface ITaskManager
{
    public Task<List<ModuleTask>> GetAllTasks();
    public Task<ModuleTask?> GetTaskById(Guid taskId);
    public Task CreateTask(ModuleTask task);
    public Task UpdateTask(ModuleTask task);
    public Task DeleteTask(Guid taskId);

    public Task<List<ModuleTask?>> GetTasksByStatus(ModuleTaskStatus status);

    public Task<List<ModuleTask?>> GetTasksByCourseId(Guid courseId);
}
