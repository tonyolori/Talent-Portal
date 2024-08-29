using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;


namespace Application.Topics.Queries
{
    public class GetAllTopicsQuery : IRequest<Result>
    {
    }

    public class GetAllTopicsQueryHandler : IRequestHandler<GetAllTopicsQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetAllTopicsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllTopicsQuery request, CancellationToken cancellationToken)
        {
            List<Topic> topics = _context.Topics.ToList();

            if (!topics.Any())
            {
                return Result.Failure("No topics found.");
            }
            
            var response = new  
            {  
                topics , 
                topicsLength = topics.Count,  
            };  

            return Result.Success<GetAllTopicsQuery>("Topics retrieved successfully.", response);
        }
    }
}