using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Questions.Queries
{
    public class GetAllAnswersByQuestionIdQuery : IRequest<Result>
    {
        public int QuestionId { get; set; }
    }

    public class GetAllAnswersByQuestionIdQueryHandler : IRequestHandler<GetAllAnswersByQuestionIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetAllAnswersByQuestionIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllAnswersByQuestionIdQuery request, CancellationToken cancellationToken)
        {
            
            List<Answer>? answers = await _context.Answers
                .Where(a => a.QuestionId == request.QuestionId)
                .ToListAsync(cancellationToken);

            if (answers == null || !answers.Any())
            {
                return Result.Failure("No answers found for the given question ID.");
            }

            return Result.Success<GetAllAnswersByQuestionIdQuery>($"All answers associated with {request.QuestionId}", answers);          }
    }
}