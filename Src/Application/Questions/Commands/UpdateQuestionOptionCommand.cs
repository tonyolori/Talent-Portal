using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Quizzes.Commands
{
    public class UpdateQuestionOptionCommand : IRequest<Result>
    {
        public int QuestionId { get; set; }
        public int CorrectOptionId { get; set; }
        public List<int> OptionIds { get; set; }
    }

    public class UpdateQuestionOptionCommandHandler : IRequestHandler<UpdateQuestionOptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdateQuestionOptionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateQuestionOptionCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the question
            Question? question = await _context.Questions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken);

            if (question == null)
            {
                return Result.Failure("Question not found.");
            }

            // Ensure all OptionIds are valid and belong to the Question
            var validOptions = await _context.Answers
                .Where(a => request.OptionIds.Contains(a.Id) && a.QuestionId == request.QuestionId)
                .ToListAsync(cancellationToken);

            if (validOptions.Count != request.OptionIds.Count)
            {
                return Result.Failure("One or more option IDs are invalid or do not belong to the specified question.");
            }

            // Ensure CorrectOptionId is part of the provided OptionIds
            if (!request.OptionIds.Contains(request.CorrectOptionId))
            {
                return Result.Failure("The correct option ID must be one of the provided option IDs.");
            }

            // Update the question
            question.CorrectOptionId = request.CorrectOptionId;

            // If you need to replace the options (in case they are different):
            question.Options = validOptions;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Question options updated successfully!");
        }
    }
}
