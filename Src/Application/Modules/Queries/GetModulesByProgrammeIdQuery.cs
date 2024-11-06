using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Domain.Entities;

namespace Application.Modules.Queries
{
    public class GetModulesByProgrammeIdQuery : IRequest<Result>
    {
        public int ProgrammeId { get; set; }
    }

    public class GetModulesByProgrammeIdQueryHandler : IRequestHandler<GetModulesByProgrammeIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetModulesByProgrammeIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetModulesByProgrammeIdQuery request, CancellationToken cancellationToken)
        {
            Programme? dbProgramme = await _context.Programmes.FindAsync(request.ProgrammeId);
            if (dbProgramme == null)
            {
                return Result.Failure("Invalid Programme");
            }

            List<Module> modules = _context.Modules.Where(
                m=> m.ProgrammeId == request.ProgrammeId).ToList();
            
            var response = new
            {
                Modules = modules,
                TotalModules = modules.Count
            };

            return Result.Success<GetModulesByProgrammeIdQuery>("Modules retrieved successfully", response);
        }
    }
}