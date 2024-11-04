using System.Text.Json.Serialization;
using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Queries
{
    public class GetModulesByInstructorIdQuery : IRequest<Result>
    {
        [Newtonsoft.Json.JsonIgnore]  
        [JsonIgnore] 
        public string InstructorId { get; set; }
    }

    public class GetModulesByInstructorIdQueryHandler : IRequestHandler<GetModulesByInstructorIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetModulesByInstructorIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetModulesByInstructorIdQuery request, CancellationToken cancellationToken)
        {
            var modules = await _context.Modules
                .Where(m => m.InstructorId == request.InstructorId)
                .ToListAsync(cancellationToken);
            
            if (!modules.Any())
            {
                return Result.Failure("No modules found for the specified instructor.");
            } 

            return Result.Success<GetModulesByInstructorIdQuery>("Modules found.", modules);
        }
    }
}