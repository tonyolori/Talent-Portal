using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Queries
{
    public class GetAllModulesQuery : IRequest<Result>
    {
    }

    public class GetAllModulesQueryHandler : IRequestHandler<GetAllModulesQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetAllModulesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllModulesQuery request, CancellationToken cancellationToken)
        {
            List<Module>? modules = await _context.Modules.ToListAsync(cancellationToken);

            if (modules == null || !modules.Any())
            {
                return Result.Failure("No modules found.");
            }

            return Result.Success(modules);
        }
    }
}