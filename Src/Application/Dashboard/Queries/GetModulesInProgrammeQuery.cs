﻿
using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Dashboard.Queries;

public class GetModulesInProgrammeQuery : IRequest<Result>
{
    public int Id;
}

public class GetModulesInProgrammeQueryHandler : IRequestHandler<GetModulesInProgrammeQuery, Result>
{
    private readonly IApplicationDbContext _context;

    public GetModulesInProgrammeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(GetModulesInProgrammeQuery request, CancellationToken cancellationToken)
    {
        List<Module>? modules = await _context.Modules
            .Include(m=>m.Topics)
            .ToListAsync(cancellationToken);

        List<Module>? filteredModules = modules.Where((m) => m.ProgrammeId == request.Id).ToList();

        if (filteredModules == null || filteredModules.Count == 0)
        {
            return Result.Success("No modules found.");
        }

        return Result.Success<GetModulesInProgrammeQuery>("Modules retrieved successfully.", filteredModules);
    }
}
