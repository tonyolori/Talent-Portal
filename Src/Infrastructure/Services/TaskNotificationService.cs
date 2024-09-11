using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;
public class TaskNotificationService(IServiceProvider serviceProvider, IApplicationDbContext context) : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IApplicationDbContext _context = context;
    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1)); // Adjust the interval as needed
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        //using var scope = _serviceProvider.CreateScope();
        //var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        List<ModuleTask> overdueTasks = await GetOverdueTasks();

        // Send notifications for each overdue task
        foreach (var task in overdueTasks)
        {
            SendNotification(task);
        }
    }
    private async Task<List<ModuleTask>> GetOverdueTasks()
    {
        DateTime today = DateTime.Now;
        return await (from task in _context.Tasks
                      where task.DueDate < today &&
                            !_context.SubmissionDetails.Any(s => s.TaskId == task.Id && s.TaskStatus == ModuleTaskStatus.NotSubmitted)
                      select task).ToListAsync();
    }

    private async Task<List<SubmissionDetails>> GetGradedSubmissions()
    {
        return await _context.SubmissionDetails
            .Where(s => s.Grade != null)
            .ToListAsync();
    }

    private async Task<List<SubmissionDetails>> GetSubmissionsWithFeedback()
    {
        return await _context.SubmissionDetails
            .Where(s => s.FacilitatorFeedBack != null)
            .ToListAsync();
    }

    private void SendNotification(ModuleTask task)
    {
        //Notification notification = new()
        //{
        //    Title = // stopped here
        //};
        //_context.Notifications.Add(notification);
        //_context.SaveChangesAsync(CancellationToken.None);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

