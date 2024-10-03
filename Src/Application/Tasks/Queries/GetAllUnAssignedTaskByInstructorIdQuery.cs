using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries
{
    public class GetAllUnAssignedTaskByInstructorIdQuery : IRequest<Result>
    {
        public string InstructorId { get; set; }
    }

    public class GetAllUnAssignedTaskByInstructorIdQueryHandler : IRequestHandler<GetAllUnAssignedTaskByInstructorIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetAllUnAssignedTaskByInstructorIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllUnAssignedTaskByInstructorIdQuery request, CancellationToken cancellationToken)
        {
            // Fetch tasks that are unassigned based on the absence of submissions
            List<ModuleTask>? unassignedTasks = await _context.Tasks
                .Where(t => t.InstructorId == request.InstructorId)
                .GroupJoin(
                    _context.SubmissionDetails,
                    task => task.Id,
                    submission => submission.TaskId,
                    (task, submissions) => new { task, submissions }
                )
                .Where(x => !x.submissions.Any())  // Select tasks with no submissions
                .Select(x => x.task)  // Only return the task objects
                .ToListAsync(cancellationToken);

            if (!unassignedTasks.Any())
            {
                return Result.Failure<GetAllUnAssignedTaskByInstructorIdQuery>($"No unassigned tasks found for Instructor ID {request.InstructorId}.");
            }

            return Result.Success<GetAllUnAssignedTaskByInstructorIdQuery>("Unassigned tasks retrieved successfully.", unassignedTasks);
        }
    }
}