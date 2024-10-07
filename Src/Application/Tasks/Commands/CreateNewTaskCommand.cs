using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Application.Tasks.Queries;

public class CreateNewTaskCommand : IRequest<Result>
{
    public string Title { get; set; }
    public required string Description { get; set; }
    public required string Instructions { get; set; }
    public int ModuleId { get; set; }
    
    public string InstructorId { get; set; }
    public DateTime DueDate { get; set; }

    
}


public class CreateNewTaskCommandHandler(IApplicationDbContext Context) : IRequestHandler<CreateNewTaskCommand, Result>
{
    private readonly IApplicationDbContext _context = Context;

    public async Task<Result> Handle(CreateNewTaskCommand request, CancellationToken cancellationToken)
    {
        Instructor? instructor = await _context.Instructors.FindAsync(request.InstructorId);

        if (instructor == null)
        {
            return Result.Failure($"Instructor with ID {request.InstructorId} not found.");
        }
        
        bool moduleExists = await _context.Modules.AnyAsync(m => m.Id == request.ModuleId, cancellationToken);  
        if (!moduleExists)  
        {  
            return Result.Failure($"Module with ID {request.ModuleId} does not exist.");  
        }  
       
        ModuleTask moduleTask = new()
        //TODO: update the task command to reflect the fact that tasks are linked to programmes
        ModuleTask task = new()
        {
            InstructorId = instructor.Id,
            Title = request.Title,
            Description = request.Description,
            Instructions = request.Instructions,
            DueDate = request.DueDate,
        };

        await _context.Tasks.AddAsync(moduleTask);
        await _context.SaveChangesAsync(CancellationToken.None);

        return Result.Success(moduleTask);
    }
}
