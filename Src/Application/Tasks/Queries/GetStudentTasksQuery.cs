using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries;

public class GetAssignedTasksQuery : IRequest<Result>
{
    public string StudentGuid { get; set; }
}

public class GetAssignedTasksQueryHandler(UserManager<Student> userManager) : IRequestHandler<GetAssignedTasksQuery, Result>
{
    private readonly UserManager<Student> _userManager = userManager;

    public async Task<Result> Handle(GetAssignedTasksQuery request, CancellationToken cancellationToken)
    {
        Student? student = await _userManager.Users.Include(s => s.AssignedTasks)
                                 .FirstOrDefaultAsync(s => s.Id == request.StudentGuid, cancellationToken);
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

        return Result.Success(student.AssignedTasks);
    }
}
