using Application.Common.Models;
using Application.Dto;
using Application.Interfaces;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentProgressTracker.Queries;


public class GetStudentTasksQuery : IRequest<Result>
{
    public int? ProgrammeId { get; set; }
    public ModuleTaskStatus? Status { get; set; }
    public int? Week { get; set; }
    public required string StudentId {  get; set; }
}

public class GetStudentTasksQueryHandler(IApplicationDbContext context) : IRequestHandler<GetStudentTasksQuery, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(GetStudentTasksQuery request, CancellationToken cancellationToken)
    {
        List<StudentTaskDetailsDto> tasks = await _context.SubmissionDetails
       .AsNoTracking()
       .Where(sd => sd.StudentId.Equals(request.StudentId))
       .Join(
           inner: _context.Tasks,
           outerKeySelector: sd => sd.TaskId,
           innerKeySelector: mt => mt.Id,
           resultSelector: (sd, mt) => new StudentTaskDetailsDto(mt, sd))
       
         .ToListAsync(cancellationToken);


        tasks = tasks.Where(t =>
                       (request.ProgrammeId == null || t.ProgrammeId == request.ProgrammeId)
                    && (request.Status == null || t.TaskStatus == request.Status)
                    && (request.Week == null || t.Week == request.Week))
            .ToList();

        //tasks = FilterByStatus(tasks,request.Status);



        //distinct after to make sure we dont remove a valid entry during a filtering step
        List<StudentTaskDetailsDto> distinctTasks = tasks.DistinctBy(t => t.StudentId).ToList();

        return Result.Success<GetStudentTasksQuery>("Tasks retrieved successfully.", tasks);
        }


    private static List<StudentTaskDetailsDto> FilterByProgramme(List<StudentTaskDetailsDto> tasks, int? programmeId)
    {
        return tasks.Where(t => t.ProgrammeId == programmeId).ToList();
    }

    private static List<StudentTaskDetailsDto> FilterByStatus(List<StudentTaskDetailsDto> tasks, ModuleTaskStatus? status)
    {
        return tasks.Where(t => t.TaskStatus == status).ToList();
    }

    private static List<StudentTaskDetailsDto> FilterByWeek(List<StudentTaskDetailsDto> tasks, int? week)
    {
        return tasks.Where(t => t.Week == week).ToList();
    }
}





