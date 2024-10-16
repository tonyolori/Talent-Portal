
using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using Domain.Enum;
using Application.Modules.Queries;

namespace Application.Dashboard.Queries;
public class GetProgressSummaryQuery : IRequest<Result>
{
    public required string StudentId;
}

public class GetModuleOverviewQueryHandler : IRequestHandler<GetProgressSummaryQuery, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public GetModuleOverviewQueryHandler(IApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Result> Handle(GetProgressSummaryQuery request, CancellationToken cancellationToken)
    {
        User? student = await _userManager.Users.Include(s => s.AssignedTasks)
                                 .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);
        if (student == null || student.UserType != UserType.Student)
        {
            // Handle student not found scenario
            return Result.Failure("student Not found");
        }

        List<Module>? modules = await _context.Modules.Where((m) => m.ProgrammeId == student.ProgrammeId).ToListAsync(cancellationToken);
        List<ModuleTask>? tasks = await _context.Tasks.Where((m) => m.ProgrammeId == student.ProgrammeId).ToListAsync(cancellationToken);

        int totalModules = modules.Count;
        int totalModulesCompleted = 1;//dummy value, to be changed later 


        //quiz section = 

        int totalQuizzes = await GetTotalQuizzes(student.ProgrammeId);
        int totalQuizzesCompleted = 999999999; // dummy value until facilities to calculate this can be created

        int totalTasks = tasks.Count;
        int totalTasksCompleted = await GetCompletedTasks(student.Id);

        ProgressSummary progress =  new ()
        {
            TotalModules = totalModules,
            TotalModulesCompleted = totalModulesCompleted,
            TotalQuizzes = totalQuizzes,
            TotalQuizzesCompleted = totalQuizzesCompleted,
            TotalTasks = totalTasks,
            TotalTasksCompleted = totalTasksCompleted
        };


        return Result.Success<GetProgressSummaryQuery>("Progress report retrieved successfully.", progress);
    }

    private async Task<int> GetTotalQuizzes(int programmeId)
    {
        return await _context.Modules
               .Where(m=> m.ProgrammeId == programmeId)
               .Include(m => m.Quizzes)
               .SelectMany(m => m.Quizzes)
               .CountAsync();
    }

    private async Task<int> GetCompletedTasks(string studentId)
    {
        //get the tasks for the specific student
        User? student = await _userManager.Users.Include(s => s.AssignedTasks)
                                 .FirstOrDefaultAsync(s => s.Id == studentId, CancellationToken.None);

        int completedTasks = await _context.Tasks
       .Join(
           inner: _context.SubmissionDetails.Where(sd => sd.StudentId == studentId),
           outerKeySelector :mt => mt.Id,
           innerKeySelector :sd => sd.TaskId,
           resultSelector :(mt, sd) => new { ModuleTask = mt, SubmissionDetails = sd })
       .Where(mt => mt.SubmissionDetails.TaskStatus == ModuleTaskStatus.Submitted)
       .CountAsync();

        return completedTasks;
    }

    
}
