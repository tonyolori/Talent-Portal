using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Domain.Enum;


namespace Application.Tasks.Queries;

public class CreateNewTaskCommand : IRequest<Result>
{
    public string Title;
    public required string Description { get; set; }
    public required string Instructions { get; set; }
    public string? SubmissionLink { get; set; }
    public DateTime SubmissionDate { get; set; }
    public int ModuleId { get; set; } // Foreign key to Module

}

public class CreateNewTaskCommandHandler(IApplicationDbContext Context) : IRequestHandler<CreateNewTaskCommand, Result>
{
    private readonly IApplicationDbContext _context = Context;

    public async Task<Result> Handle(CreateNewTaskCommand request, CancellationToken cancellationToken)
    {
       
        ModuleTask task = new()
        {
            Guid = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Instructions = request.Instructions,
            SubmissionLink = request.SubmissionLink,
            FacilitatorFeedBack = null,
            Status = ModuleTaskStatus.NotSubmitted,
            ModuleId = 1 // Associate task with the created module
        };

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync(CancellationToken.None);

        return Result.Success(task);
    }
}
