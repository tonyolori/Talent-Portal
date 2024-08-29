using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Topics.Queries
{
    public class GetTopicByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetTopicByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
        {
            Topic? topic = await _context.Topics.FindAsync(request.Id);

            if (topic == null)
            {
                return Result.Failure("Topic not found.");
            }

            return Result.Success<GetTopicByIdQuery>("Topic found.", topic);
        }
    }
}