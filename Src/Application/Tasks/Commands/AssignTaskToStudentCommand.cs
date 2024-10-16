using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Application.Tasks.Commands;
public class AssignTaskToStudentCommand : IRequest<Result>
{
    public string StudentId { get; set; }
    public int ModuleTaskId { get; set; }
}

public class AssignTaskToStudentCommandHandler(IApplicationDbContext context, UserManager<User> userManager) : IRequestHandler<AssignTaskToStudentCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Result> Handle(AssignTaskToStudentCommand request, CancellationToken cancellationToken)
    {
        //var student = await _userManager.FindByIdAsync(request.StudentId);
        User? student = await _userManager.Users.Include(s => s.AssignedTasks)
                                 .FirstOrDefaultAsync(s => s.Id == request.StudentId && s.UserType == UserType.Student, cancellationToken);

        if (student == null)
        {
            return Result.Failure($"Student with ID {request.StudentId} not found.");
        }

        // Check if the task exists
        var task = await _context.Tasks.FindAsync(request.ModuleTaskId, cancellationToken);
        if (task == null)
        {
            return Result.Failure($"ModuleTask with ID {request.ModuleTaskId} not found.");
        }

        student.AssignedTasks ??= new List<ModuleTask>(); // Initialize if null

        if (student.AssignedTasks.Any(t => t.Id == request.ModuleTaskId))
        {
            return Result.Failure($"Student with ID {request.StudentId} is already assigned this task.");
        }

        SubmissionDetails details = new()
        {
            TaskId = request.ModuleTaskId,
            StudentId = request.StudentId,
            TaskStatus = ModuleTaskStatus.NotSubmitted,
            TaskStatusDesc = ModuleTaskStatus.NotSubmitted.ToString(),
        };

        //update the grade
        _context.SubmissionDetails.Add(details);

        student.AssignedTasks.Add(task);

        await _userManager.UpdateAsync(student);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success("");
    }
}