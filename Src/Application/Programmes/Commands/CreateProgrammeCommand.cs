using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Domain.Enum;


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

        Programme programme = new()
        {
            Type = request.Type,
        };

        await _context.Programmes.AddAsync(programme);
        await _context.SaveChangesAsync(CancellationToken.None);

        return Result.Success(programme);
    }
}


