using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;


namespace Application.Questions.Queries;

public class UpdateAnswerByIdCommand : IRequest<Result>
{
    public int AnswerId { get; set; }
    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
}

public class UpdateAnswerByIdCommandHandler : IRequestHandler<UpdateAnswerByIdCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateAnswerByIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateAnswerByIdCommand request, CancellationToken cancellationToken)
    {
        Answer? answer = await _context.Answers.FindAsync(request.AnswerId);

        if (answer == null)
        {
            return Result.Failure("Answer not found.");
        }

        answer.AnswerText = request.AnswerText;
        answer.IsCorrect = request.IsCorrect;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success("Answer updated successfully.");
    }
}
