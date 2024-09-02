using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

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
        Student? student = await _userManager.FindByIdAsync(request.StudentGuid);

        if (student == null)
        {
            // Handle student not found scenario
            return Result.Failure("student Not found");
        }

        List<ModuleTask>? assignedTasks = student.AssignedTasks?.ToList();

        if (assignedTasks == null || assignedTasks.Count == 0)
        {
            // Handle task not assigned to student scenario
            //return Result.Success(new List<Grade>());
        }

        return Result.Success(assignedTasks.ToList());
    }
}
