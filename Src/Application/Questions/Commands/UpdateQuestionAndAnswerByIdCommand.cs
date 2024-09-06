using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Questions.Commands;

public class UpdateQuestionAndAnswerByIdCommand : IRequest<Result>
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
    public List<UpdateDetails> Options { get; set; }
}

public class UpdateDetails
{
    public int? AnswerId { get; set; } // Nullable to allow for new answers (no ID yet)
    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
}

public class UpdateQuestionAndAnswerByIdCommandHandler : IRequestHandler<UpdateQuestionAndAnswerByIdCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateQuestionAndAnswerByIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateQuestionAndAnswerByIdCommand request, CancellationToken cancellationToken)
    {
        Question? question = await _context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken);

        if (question == null)
        {
            return Result.Failure("Question not found.");
        }

        // Update question text
        question.QuestionText = request.QuestionText;

        // Update or add answers
        foreach (var option in request.Options)
        {
            Answer? existingAnswer = question.Options.FirstOrDefault(a => a.Id == option.AnswerId);
            if (existingAnswer != null)
            {
                // Update existing answer
                existingAnswer.AnswerText = option.AnswerText;
                existingAnswer.IsCorrect = option.IsCorrect;
            }
            else
            {
                // Add new answer
                question.Options.Add(new Answer
                {
                    AnswerText = option.AnswerText,
                    IsCorrect = option.IsCorrect,
                    QuestionId = request.QuestionId
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success<UpdateQuestionAndAnswerByIdCommand>("Question and answers updated successfully.", question);
    }
}
