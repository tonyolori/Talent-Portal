using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Questions.Commands
{
    public class CreateQuestionCommand : IRequest<Result>
    {
        public string InstructorId { get; set; } // To identify instructor
        public int? ModuleId { get; set; } // ModuleId is optional
        public List<QuestionInput> Questions { get; set; } // List of questions with options
    }

    public class QuestionInput
    {
        public string QuestionText { get; set; }
        public List<OptionInput> Options { get; set; } // List of options for each question
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
            // Check if a quiz exists for the instructor and module
            Quiz quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.ModuleId == request.ModuleId && q.InstructorId == request.InstructorId, cancellationToken);

            // If no quiz exists, create a new one
            if (quiz == null)
            {
                if (request.ModuleId == null)
                {
                    return Result.Failure("ModuleId is required for creating a quiz.");
                }

                quiz = new Quiz
                {
                    QuizStatus = QuizStatus.NotStarted,
                    QuizStatusDes = "NotStarted",
                    InstructorId = request.InstructorId,
                    ModuleId = request.ModuleId.Value
                };

                await _context.Quizzes.AddAsync(quiz, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken); // Save to generate QuizId
            }

            // Iterate over the questions and add them to the quiz
            foreach (var questionInput in request.Questions)
            {
                // Ensure there's at least one correct answer for each question
                if (!questionInput.Options.Any(o => o.IsCorrect))
                {
                    return Result.Failure($"There must be at least one correct answer for question: {questionInput.QuestionText}.");
                }

                // Create the question entity
                var question = new Question
                {
                    QuestionText = questionInput.QuestionText,
                    QuizId = quiz.Id
                };

                // Add options (answers) to the question
                foreach (var option in questionInput.Options)
                {
                    Option answer = new()
                    {
                        AnswerText = option.AnswerText,
                        IsCorrect = option.IsCorrect,
                        Question = question
                    };

                    question.Options.Add(answer);
                }

                // Add the question to the context
                _context.Questions.Add(question);
            }

            // Save changes
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Questions and options created successfully.");
        }
    }
}
