using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;


namespace Application.Questions.Commands;

public class UpdateQuestionByIdCommand : IRequest<Result>
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
}

public class UpdateQuestionByIdCommandHandler : IRequestHandler<UpdateQuestionByIdCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateQuestionByIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateQuestionByIdCommand request, CancellationToken cancellationToken)
    {
        Question? question = await _context.Questions.FindAsync(request.QuestionId);

        if (question == null)
        {
            return Result.Failure("Question not found.");
        }

        question.QuestionText = request.QuestionText;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success("Question updated successfully.");
    }
}
