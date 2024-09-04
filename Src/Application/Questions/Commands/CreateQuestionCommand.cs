using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Questions.Commands
{
public class CreateQuestionCommand : IRequest<Result>
{
    public string QuestionText { get; set; }
    public int QuizId { get; set; }
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
        var question = new Question
        {
            QuestionText = request.QuestionText,
            QuizId = request.QuizId
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success<CreateQuestionCommand>("Question created successfully!", question);
    }
}

}