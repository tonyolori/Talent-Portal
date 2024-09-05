using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries;

public class GetTaskGradeQuery : IRequest<Result>
{
    public string StudentId { get; set; }
    public int ModuleTaskId { get; set; }
}

public class GetTaskGradeQueryHandler(UserManager<Student> userManager, IApplicationDbContext context) : IRequestHandler<GetTaskGradeQuery, Result>
{
    private readonly UserManager<Student> _userManager = userManager;
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(GetTaskGradeQuery request, CancellationToken cancellationToken)
    {
        Student? student = await _userManager.Users
            .Include(s => s.AssignedTasks)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
        {
            return Result.Failure("Student Not Found");
        }

        if (!student.AssignedTasks.Any(t => t.Id == request.ModuleTaskId))
        {
            return Result.Failure<float>("Task Not Found for Student");
        }

        SubmissionDetails? submissionDetails = _context.SubmissionDetails.FirstOrDefault(s => s.TaskId == request.ModuleTaskId && s.StudentId == request.StudentId);

        if (submissionDetails == null)
        {
            // create new details if null
            SubmissionDetails details = new()
            {
                TaskId = request.ModuleTaskId,
                StudentId = request.StudentId,
            };
            return Result.Failure<float>("No Submission Available");
        }

        return Result.Success(submissionDetails.Grade);
    
    }
}