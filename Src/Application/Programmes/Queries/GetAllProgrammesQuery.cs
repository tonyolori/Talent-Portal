using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.Dto;

namespace Application.Programmes.Queries;

public class GetAllProgrammesQuery : IRequest<Result>
{

}

public class GetAllProgrammesQueryHandler(IApplicationDbContext context,IMapper mapper) : IRequestHandler<GetAllProgrammesQuery, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result> Handle(GetAllProgrammesQuery request, CancellationToken cancellationToken)
    {
        List<Programme>? Programmes = await _context.Programmes.ToListAsync();

        if (Programmes == null)
        {
            return Result.Failure<GetAllProgrammesQuery>("No Programmes");
        }

        List<ProgrammeDto> programmeDtos = [];

        foreach (Programme programme in Programmes)
        {
            programmeDtos.Add(_mapper.Map<ProgrammeDto>(programme));
        }
        // Return the user or null if not found
        return Result.Success<GetAllProgrammesQuery>("Programmes retrieved successfully.", programmeDtos);
    }
}
