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

            if (modules == null || modules.Count == 0)
            {
                return Result.Success("No modules found.");
            }

            var response = new
            {
                modules,
                modulesLength = modules.Count
            };

           
            return Result.Success<GetAllModulesQuery>("Modules retrieved successfully.", response);
        }
    }
}