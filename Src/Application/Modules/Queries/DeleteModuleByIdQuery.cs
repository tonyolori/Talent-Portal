using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Domain.Entities;

namespace Application.Modules.Queries
{
    public class DeleteModuleByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteModuleByIdQueryHandler : IRequestHandler<DeleteModuleByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public DeleteModuleByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteModuleByIdQuery request, CancellationToken cancellationToken)
        {
            Module? module = await _context.Modules.FindAsync(request.Id, cancellationToken);
            
            if (module == null)
            {
                return Result.Failure("Module not found.");
            }

            _context.Modules.Remove(module);

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success("Module Deleted Successfully");
        }
    }
}