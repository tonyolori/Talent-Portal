using Application.Common.Models;
using Application.Interfaces;
using MediatR;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Commands
{
    public class UpdateModuleStatusCommand : IRequest<Result>
    {
        public int ModuleId { get; set; }
        public ModuleStatus ModuleStatus { get; set; }  
    }

    public class UpdateModuleStatusCommandHandler : IRequestHandler<UpdateModuleStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdateModuleStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateModuleStatusCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the module by ID
            Module? module = await _context.Modules
                .FirstOrDefaultAsync(m => m.Id == request.ModuleId, cancellationToken);

            if (module == null)
            {
                return Result.Failure("Module not found.");
            }

            // Update the module status
            module.ModuleStatus =  request.ModuleStatus;
            module.ModuleStatusDes = request.ModuleStatus.ToString(); 

            _context.Modules.Update(module);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<UpdateModuleStatusCommand>($"Module status updated to {request.ModuleStatus}.", module);
        }
    }
}