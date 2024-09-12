using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace Infrastructure.Services;
public class TaskNotificationService(IServiceProvider serviceProvider) : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    //private readonly IApplicationDbContext _context = context;
    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)); // Adjust the interval as needed
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        List<Tuple<ModuleTask, string>> overdueTasks = await GetOverdueTasks(context);

        // Send notifications for each overdue task
        foreach (var taskData in overdueTasks)
        {
            ModuleTask task = taskData.Item1;
            string studentId = taskData.Item2;
            TimeSpan timeRemaining = task.DueDate - DateTime.Now;

            int days = Math.Max(0, timeRemaining.Days);
            int hours = Math.Max(0, timeRemaining.Hours);
            int minutes = Math.Max(0, timeRemaining.Minutes);

            string timeLeftMessage;

            if (days > 0)
            {
                timeLeftMessage = $"{days} day{(days > 1 ? "s" : "")}";
            }
            else if (hours > 0)
            {
                timeLeftMessage = $"{hours} hour{(hours > 1 ? "s" : "")}";
            }
            else
            {
                timeLeftMessage = $"{minutes} minute{(minutes > 1 ? "s" : "")}";
            }
            string message = $"Your task '{task.Title}'  would be due in {timeLeftMessage}";

            Notification notification = new()
            {
                Title = "Overdue task",
                ShortMessage = message,
                LongMessage = message + """
                        You are strongly advised to submit before the deadline. 

                        Keep studying 

                        Cheers, mate
                    """,
                TaskId = task.Id,
                StudentId = studentId,//get student id and place it here,

            };


            await SendNotification(notification, context);


            //now log it
            Notification notificationlog = new()
            {
                Title = "log",
                ShortMessage = "log",
                LongMessage = message + """
                        
                    """,
                TaskId = task.Id,
                StudentId = studentId + " log",//get student id and place it here,

            };

            await SendNotification(notificationlog, context);

            //stop after 1 execution
            _timer?.Change(Timeout.Infinite, 0);
        }
    }

    private async Task<List<Tuple<ModuleTask, string>>> GetOverdueTasks(IApplicationDbContext context)
    {
        DateTime today = DateTime.Now;
        //var results = await (from task in context.Tasks
        //              join submission in context.SubmissionDetails on task.Id equals submission.TaskId
        //              where task.DueDate < today &&
        //                     submission.TaskStatus == ModuleTaskStatus.NotSubmitted
        //              select new { task, submission.StudentId })
        //              .DistinctBy(t => t.task.Id) // Ensure unique tasks
        //              //.Select(t => t.task) // Extract only the ModuleTask
        //              .ToListAsync();

        var results = context.Tasks
            .Join(
                inner: context.SubmissionDetails,
                outerKeySelector: task => task.Id,
                innerKeySelector: submission => submission.TaskId,
                resultSelector: (task, submission) => new
                {
                    task,
                    submission
                })
            .Where(x => x.task.DueDate < today && (int)x.submission.TaskStatus == 1)
            .Select(x => new
            {
                x.task,
                x.submission.StudentId
            })
            //.DistinctBy(t => t.task.Id)
            .ToList();

        return results.Select(result => Tuple.Create(result.task, result.StudentId)).ToList();
      //return results.Select(result => Tuple.Create(result.x, "jj")).ToList();
    }

    //private async Task<List<ModuleTask>> GetOverdueTasks(IApplicationDbContext context)
    //{
    //    DateTime today = DateTime.Now;
    //    return await (from task in context.Tasks
    //                  where task.DueDate < today &&
    //                        !context.SubmissionDetails.Any(s => s.TaskId == task.Id && s.TaskStatus == ModuleTaskStatus.NotSubmitted)
    //                  select task).ToListAsync();
    //}

    //private async Task<List<SubmissionDetails>> GetGradedSubmissions()
    //{
    //    return await context.SubmissionDetails
    //        .Where(s => s.Grade != null)
    //        .ToListAsync();
    //}

    //private async Task<List<SubmissionDetails>> GetSubmissionsWithFeedback()
    //{
    //    return await context.SubmissionDetails
    //        .Where(s => s.FacilitatorFeedBack != null)
    //        .ToListAsync();
    //}

    private async Task SendNotification(Notification notification, IApplicationDbContext context)
    {
        // Check if a notification already exists for the same StudentId and TaskId
        Notification? existingNotification = await context.Notifications
            .FirstOrDefaultAsync(n => n.StudentId == notification.StudentId && n.TaskId == notification.TaskId);

        if (existingNotification != null)
        {
            // Update the existing notification if it exists
            existingNotification.ShortMessage = notification.ShortMessage;
            existingNotification.LongMessage = notification.LongMessage;
            existingNotification.DateCreated = DateTime.Now;
        }
        else
        {
            // Add a new notification if it doesn't exist
            context.Notifications.Add(notification);
        }

        await context.SaveChangesAsync(CancellationToken.None);
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

