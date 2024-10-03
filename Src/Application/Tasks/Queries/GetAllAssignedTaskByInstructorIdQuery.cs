using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries
{
    public class GetAllAssignedTaskByInstructorIdQuery : IRequest<Result>
    {
        public string InstructorId { get; set; }
    }

    public class GetAllAssignedTaskByInstructorIdQueryHandler : IRequestHandler<GetAllAssignedTaskByInstructorIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetAllAssignedTaskByInstructorIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllAssignedTaskByInstructorIdQuery request, CancellationToken cancellationToken)
        {
            // Fetch tasks assigned to students based on instructor's ID
            var assignedTasks = await _context.Tasks
                .Where(t => t.InstructorId == request.InstructorId)
                .Join(
                    _context.SubmissionDetails,
                    task => task.Id,
                    submission => submission.TaskId,
                    (task, submissions) => new { task, submissions }
                )
                .Distinct()
                .ToListAsync(cancellationToken);

            if (assignedTasks.Count == 0)
            {
                return Result.Failure($"No assigned tasks found for Instructor ID {request.InstructorId}");
            }

            return Result.Success("Assigned tasks retrieved successfully.", assignedTasks);
        }
    }
}
