using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Notifications.Queries;

public class GetUnreadNotificationsQuery : IRequest<Result>
{
    public required string StudentId;
}


public class GetUnreadNotificationsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetUnreadNotificationsQuery, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
    {
        List<Notification>? notifications = await _context.Notifications
            .Where(n => n.StudentId == request.StudentId && !n.IsRead).ToListAsync();

        if (notifications == null)
        {
            return Result.Success<GetUnreadNotificationsQuery>("No notifications");
        }

        // Return the user or null if not found
        return Result.Success<GetUnreadNotificationsQuery>("Notifications retrieved successfully.", notifications);
    }
}

