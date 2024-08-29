using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Commands;
using Domain.Common.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.Topics.Commands
{
    public class CreateTopicCommand : IRequest<Result>
    {
        public string Title { get; set; }
        public string MainContent { get; set; }
        public string? SubContent { get; set; }
        public TopicStatus Status { get; set; }
        public int ModuleId { get; set; }
    }

    public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateTopicCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            
            bool moduleExists = await _context.Modules.AnyAsync(m => m.Id == request.ModuleId, cancellationToken);  
            if (!moduleExists)  
            {  
                return Result.Failure($"Module with ID {request.ModuleId} does not exist.");  
            }  
           
            Topic topic = new ()
            {
                Title = request.Title,
                MainContent = request.MainContent,
                SubContent = request.SubContent,
                Status = request.Status,
                ModuleId = request.ModuleId
            };

            await _context.Topics.AddAsync(topic, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<CreateModuleCommand>("Topic created successfully!", topic);
        }
    }
}