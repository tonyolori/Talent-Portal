using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Application.Programmes.Commands;

public class CreateProgrammeCommand : IRequest<Result>
{
    public String Type { get; set; }

}

public class CreateProgrammeCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateProgrammeCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(CreateProgrammeCommand request, CancellationToken cancellationToken)
    {
        // Check if the programme exists
        Programme? existingProgramme = await _context.Programmes
            .FirstOrDefaultAsync(p => p.Type == request.Type, cancellationToken);

        if (existingProgramme != null)
        {
            return Result.Failure($"Programme '{request.Type}'  already exist");
        }

        Programme programme = new()
        {
            Type = request.Type,
        };

        await _context.Programmes.AddAsync(programme);
        await _context.SaveChangesAsync(CancellationToken.None);

        return Result.Success(programme);
    }
}


