using System.Text.Json.Serialization;
using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Modules.Queries
{
    public class GetModuleByInstructorIdQuery : IRequest<Result>
    {
        [Newtonsoft.Json.JsonIgnore]  
        [JsonIgnore] 
        public string InstructorId { get; set; }
    }

    public class GetModuleByInstructorIdQueryHandler : IRequestHandler<GetModuleByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetModuleByInstructorIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetModuleByIdQuery request, CancellationToken cancellationToken)
        {

            List<Module> module = _context.Modules.Where(
                m=> m.InstructorId == request.InstructorId).ToList();
            
            if (module == null)
            {
                return Result.Failure("Module not found.");
            } 

            return Result.Success<GetModuleByIdQuery>("Module found.", module);
        }
    }
}