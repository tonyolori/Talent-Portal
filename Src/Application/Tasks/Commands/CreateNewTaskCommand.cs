using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;


namespace Application.Tasks.Queries;

public class CreateNewTaskCommand : IRequest<Result>
{
    public string Title { get; set; }
    public required string Description { get; set; }
    public required string Instructions { get; set; }
    public int ModuleId { get; set; } // Foreign key to ModuleController

}

public class CreateNewTaskCommandHandler(IApplicationDbContext Context) : IRequestHandler<CreateNewTaskCommand, Result>
{
    private readonly IApplicationDbContext _context = Context;

    public async Task<Result> Handle(CreateNewTaskCommand request, CancellationToken cancellationToken)
    {
        bool moduleExists = await _context.Modules.AnyAsync(m => m.Id == request.ModuleId, cancellationToken);  
        if (!moduleExists)  
        {  
            return Result.Failure($"Module with ID {request.ModuleId} does not exist.");  
        }  
       
        ModuleTask task = new()
        {
            Title = request.Title,
            Description = request.Description,
            Instructions = request.Instructions,
            ModuleId = request.ModuleId // Associate task with the created module
        };

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync(CancellationToken.None);

        return Result.Success(task);
    }
}
