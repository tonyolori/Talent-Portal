using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Modules.Queries
{
    public class GetModuleByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetModuleByIdQueryHandler : IRequestHandler<GetModuleByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetModuleByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetModuleByIdQuery request, CancellationToken cancellationToken)
        {
            Module? module = await _context.Modules.FindAsync(new object[] { request.Id }, cancellationToken);

            if (module == null)
            {
                return Result.Failure("Module not found.");
            } 

            return Result.Success<GetModuleByIdQuery>("Module found.", module);
        }
    }
}