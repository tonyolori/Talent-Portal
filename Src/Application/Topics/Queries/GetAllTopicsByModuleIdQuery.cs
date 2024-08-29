using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Domain.Common.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.Topics.Queries
{
    public class GetAllTopicsByModuleIdQuery : IRequest<Result>
    {
        public int ModuleId { get; set; }
        public TopicStatus? Status { get; set; } 
    }

    public class GetAllTopicsByModuleIdQueryHandler : IRequestHandler<GetAllTopicsByModuleIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetAllTopicsByModuleIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllTopicsByModuleIdQuery request, CancellationToken cancellationToken)
        {
            // Check if the module exists
            bool moduleExists = await _context.Modules.AnyAsync(m => m.Id == request.ModuleId, cancellationToken);
            if (!moduleExists)
            {
                return Result.Failure($"Module with ID {request.ModuleId} does not exist.");
            }

            // Query topics with optional status filter
            IQueryable<Topic> query = _context.Topics.Where(t => t.ModuleId == request.ModuleId);

            if (request.Status.HasValue)
            {
                query = query.Where(t => t.Status == request.Status.Value);
            }

            List<Topic> topics = await query.ToListAsync(cancellationToken);

            if (!topics.Any())
            {
                return Result.Failure($"No topics found for ModuleId {request.ModuleId}.");
            }

            var response = new
            {
                Topics = topics,
                TopicsLength = topics.Count,
            };

            return Result.Success<GetAllTopicsByModuleIdQuery>("Topics retrieved successfully.", response);
        }
    }
}
