using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Tasks.Queries
{
    public class GetAllTaskByInstructorIdQuery : IRequest<Result>
    {
        public string InstructorId { get; set; }
    }

    public class GetAllTaskByInstructorIdQueryHandler : IRequestHandler<GetAllTaskByInstructorIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetAllTaskByInstructorIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllTaskByInstructorIdQuery request, CancellationToken cancellationToken)
        {
            List<ModuleTask>? tasks = await _context.Tasks
                .Where(t => t.InstructorId == request.InstructorId)
                .ToListAsync(cancellationToken);

            if (!tasks.Any() || tasks.Count == 0)
            {
                return Result.Failure($"No tasks found for Instructor ID {request.InstructorId}.");
            }

            return Result.Success<GetAllTaskByInstructorIdQuery>("Tasks retrieved successfully.", tasks);
        }
    }
}