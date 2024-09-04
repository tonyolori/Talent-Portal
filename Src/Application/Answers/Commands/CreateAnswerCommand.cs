using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Answers.Commands
{
    public class CreateAnswerCommand : IRequest<Result>
    {
        public string AnswerText { get; set; }
        public int QuestionId { get; set; }
    }

    public class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateAnswerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
        {
            // Check if the QuestionId exists in the database  
            var questionExists = await _context.Questions
                .AnyAsync(q => q.Id == request.QuestionId, cancellationToken);

            // If the question does not exist, return an error  
            if (!questionExists)
            {
                return Result.Failure("The specified QuestionId does not exist.");
            }

            // Check the current number of answers for the question
            var answerCount = await _context.Answers
                .CountAsync(a => a.QuestionId == request.QuestionId, cancellationToken);

            // If there are already 4 answers, return a failure
            if (answerCount >= 4)
            {
                return Result.Failure("A question can only have 4 answers.");
            }

            // Create and add the new answer
            var answer = new Answer
            {
                AnswerText = request.AnswerText,
                QuestionId = request.QuestionId
            };

            _context.Answers.Add(answer);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<CreateAnswerCommand>("Answer created successfully!", answer);
        }
    }
}