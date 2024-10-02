using Application.Common.Models;
using Application.Dto;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentProgressTracker.Queries;
public class GetStudentModulesQuery : IRequest<Result>
{
    public required string StudentId { get; set; }
    public required int ProgrammeId { get; set; }
}

public class GetStudentModulesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetStudentModulesQuery, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(GetStudentModulesQuery request, CancellationToken cancellationToken)
    {
        List<StudentModuleDetailsDto> modules = await _context.Modules
       .AsNoTracking()
       .Where(m => m.ProgrammeId.Equals(request.ProgrammeId))
       .GroupJoin(
           inner: _context.StudentModules.Where(m => m.StudentId.Equals(request.StudentId)),
           innerKeySelector: sm => sm.ModuleId,
           outerKeySelector: md => md.Id,
         resultSelector: (md, sm) => new StudentModuleDetailsDto(md, sm.FirstOrDefault()))
         .ToListAsync(cancellationToken);

        return Result.Success<GetStudentModulesQuery>("Modules retrieved successfully.",modules);
    }

    
}





