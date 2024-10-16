using Application.Common.Models;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries;

public class GetAssignedTasksQuery : IRequest<Result>
{
    public string StudentId { get; set; }
}

public class GetAssignedTasksQueryHandler(UserManager<User> userManager) : IRequestHandler<GetAssignedTasksQuery, Result>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Result> Handle(GetAssignedTasksQuery request, CancellationToken cancellationToken)
    {
        User? student = await _userManager.Users.Include(s => s.AssignedTasks)
                                 .FirstOrDefaultAsync(s => s.Id == request.StudentId && s.UserType == UserType.Student, cancellationToken);
        if (student == null)
        {
            // Handle student not found scenario
            return Result.Failure("student Not found");
        }

        if (student.AssignedTasks == null )
        {
            // Handle task not assigned to student scenario
            return Result.Success("");
        }
        //List<ModuleTask>? assignedTasks =.ToList();

        //return Result.Success(student.AssignedTasks);
        // Order the assigned tasks by their created date (ascending)
        List<ModuleTask> orderedTasks = student.AssignedTasks.OrderBy(t => t.CreatedDate).ToList();

        return Result.Success(orderedTasks);
    }
}
