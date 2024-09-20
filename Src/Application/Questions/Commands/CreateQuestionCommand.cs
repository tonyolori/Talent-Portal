using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Questions.Commands
{
    
    public class CreateQuestionCommand : IRequest<Result>
    {
        public string QuestionText { get; set; }
        public List<OptionInput> Options { get; set; }
        public int QuizId { get; set; }
    }

    public class OptionInput
    {
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateQuestionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            // Check if the quiz exists
            bool quizExists = await _context.Quizzes.AnyAsync(q => q.Id == request.QuizId, cancellationToken);
            if (!quizExists)
            {
                return Result.Failure("Quiz not found.");
            }

            // Ensure there's at least one correct answer
            if (!request.Options.Any(o => o.IsCorrect))
            {
                return Result.Failure("There must be at least one correct answer.");
            }

            // Create the question entity
            var question = new Question
            {
                QuestionText = request.QuestionText,
                QuizId = request.QuizId
            };

            // Add options (answers) to the question
            foreach (var option in request.Options)
            {
                Answer answer = new ()
                {
                    AnswerText = option.AnswerText,
                    IsCorrect = option.IsCorrect,
                    Question = question
                };

                question.Options.Add(answer);
            }

            // Add question to context
            _context.Questions.Add(question);

            // Save changes
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<CreateQuestionCommand>("Question and options created successfully.", question);
        }
    }
}
