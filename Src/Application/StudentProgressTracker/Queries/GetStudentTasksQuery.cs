using Application.Common.Models;
using Application.Interfaces;
using Domain.Common.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentProgressTracker.Queries;


public class GetStudentTasksQuery : IRequest<Result>
{
    public int? ProgrammeId { get; set; }
    public ModuleTaskStatus? Status { get; set; }
    public int? Week { get; set; }

}

public class GetStudentTasksQueryHandler(IApplicationDbContext context) : IRequestHandler<GetStudentTasksQuery, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(GetStudentTasksQuery request, CancellationToken cancellationToken)
    {
        List<StudentTaskDetails> tasks = await _context.Tasks
       .Join(
           inner: _context.SubmissionDetails,
           outerKeySelector: mt => mt.Id,
           innerKeySelector: sd => sd.TaskId,
           resultSelector: (mt, sd) => new StudentTaskDetails(mt,sd))
         .ToListAsync(cancellationToken);


        if (request.ProgrammeId != null)
        {
            tasks = FilterByProgramme(tasks, request.ProgrammeId);
        }

        if (request.Status != null)
        {
            tasks = FilterByStatus(tasks, request.Status);
        }

        if (request.ProgrammeId != null)
        {
            tasks = FilterByWeek(tasks, request.Week);
        }

        //distinct after to make sure we dont remove a valid entry during a filtering step
        List<StudentTaskDetails> distinctTasks = tasks.DistinctBy(t => t.StudentId).ToList();

        return Result.Success<GetStudentTasksQuery>("Tasks retrieved successfully.", tasks);
        }

    private static List<StudentTaskDetails> FilterByProgramme(List<StudentTaskDetails> tasks, int? programmeId)
    {
        return tasks.Where(t=>t.ProgrammeId == programmeId).ToList();
    }

    private static List<StudentTaskDetails> FilterByStatus(List<StudentTaskDetails> tasks, ModuleTaskStatus? status)
    {
        return tasks.Where(t => t.TaskStatus == status).ToList();
    }

    private static List<StudentTaskDetails> FilterByWeek(List<StudentTaskDetails> tasks, int? week)
    {
        return tasks.Where(t => t.Week == week).ToList();
    }

}





