using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries;

public class GetTaskByIdQuery : IRequest<Result>
{
    public int TaskId { get; set; }
}

public class GetTaskByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTaskByIdQuery, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        // Retrieve the task without submission details
        ModuleTask? task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task == null)
        {
            return Result.Failure($"Task with ID {request.TaskId} not found.");
        }

        // Check if submission details exist
        List<SubmissionDetails> submissionDetails = await _context.SubmissionDetails
            .Where(s => s.TaskId == task.Id)
            .ToListAsync(cancellationToken);

        if (submissionDetails.Any())
        {
            var result = new
            {
                Task = task,
                SubmissionDetails = submissionDetails
            };

            return Result.Success<GetTaskByIdQuery>("Task and SubmissionDetails retrieved successfully.", result);
        }

        // If no submission details, just return the task
        return Result.Success<GetTaskByIdQuery>("Task retrieved successfully.", task);
    }
}