using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class NotificationService
{
    private readonly IApplicationDbContext _dbContext;

    public NotificationService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateNotification(Notification notification)
    {
        _dbContext.Notifications.Add(notification);
        _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public List<Notification> GetUnreadNotifications(int studentId)
    {
        return _dbContext.Notifications.Where(n => n.StudentId == studentId && !n.IsRead).ToList();
    }
}
