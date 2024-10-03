using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Services;
public class TaskNotificationService(IServiceProvider serviceProvider) : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private Timer? _timer;

    public System.Threading.Tasks.Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30000));

        return System.Threading.Tasks.Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        //First log it
        DateTime time = DateTime.UtcNow;
        Random random = new();
        int processID = random.Next(9999999, 999999999);
        Notification notificationlog = new()
        {
            Title = "Server Log",
            ShortMessage = "The server ran at " + time.ToShortTimeString(),
            LongMessage = "The server ran at " + time.ToLongDateString() + ", " + time.ToLongTimeString() + $" process Id = {processID}",
        };

        await SendNotification(notificationlog, context);

        List<(ModuleTask, string)> overdueTasks = await GetOverdueTasks(context);

        // Send notifications for each overdue task
        foreach (var taskData in overdueTasks)
        {
            ModuleTask moduleTask = taskData.Item1;
            string studentId = taskData.Item2;
            TimeSpan timeRemaining = moduleTask.DueDate - DateTime.UtcNow;

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
            string message = $"Your task '{moduleTask.Title}'  would be due in {timeLeftMessage}";

            Notification notification = new()
            {
                Title = "Overdue task",
                ShortMessage = message,
                LongMessage = message + """
                        You are strongly advised to submit before the deadline. 

                        Keep studying 

                        Cheers, mate
                    """,
                TaskId = moduleTask.Id,
                StudentId = studentId,//get student id and place it here,

            };


            await SendNotification(notification, context);

            //stop after 1 execution
            await StopAsync(CancellationToken.None);
        }
    }

    private async Task<List<(ModuleTask, string)>> GetOverdueTasks(IApplicationDbContext context)
    {
        DateTime today = DateTime.UtcNow;
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
            .ToList();

        return results.Select(result => (result.task,result.submission.StudentId)).ToList();
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

    private async System.Threading.Tasks.Task SendNotification(Notification notification, IApplicationDbContext context)
    {
        //TESTING PURPOSES, if student ID is empty just add and dont worry about duplicates
        if(notification.StudentId == "")
        {
            context.Notifications.Add(notification);
            await context.SaveChangesAsync(CancellationToken.None);
            return;
        }


        // Check if a notification already exists for the same StudentId and TaskId
        Notification? existingNotification = await context.Notifications
            .FirstOrDefaultAsync(n => n.StudentId == notification.StudentId && n.TaskId == notification.TaskId);
        
        if (existingNotification != null)
        {
            // Update the existing notification if it exists
            existingNotification.ShortMessage = notification.ShortMessage;
            existingNotification.LongMessage = notification.LongMessage;
            existingNotification.DateCreated = DateTime.UtcNow;
        }
        else
        {
            // Add a new notification if it doesn't exist
            context.Notifications.Add(notification);
        }

        await context.SaveChangesAsync(CancellationToken.None);
    }

    public System.Threading.Tasks.Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return System.Threading.Tasks.Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

