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
    public System.Threading.Tasks.Task CreateTask(ModuleTask moduleTask);
    public System.Threading.Tasks.Task UpdateTask(ModuleTask moduleTask);
    public System.Threading.Tasks.Task DeleteTask(Guid taskId);

    public Task<List<ModuleTask?>> GetTasksByStatus(ModuleTaskStatus status);

    public Task<List<ModuleTask?>> GetTasksByCourseId(Guid courseId);
}
