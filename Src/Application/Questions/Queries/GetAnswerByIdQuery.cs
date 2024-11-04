using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Questions.Queries;
public class GetAnswerByIdQuery : IRequest<Result>
{
    public int AnswerId { get; set; }
}

public class GetAnswerByIdQueryHandler : IRequestHandler<GetAnswerByIdQuery, Result>
{
    private readonly IApplicationDbContext _context;

    public GetAnswerByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(GetAnswerByIdQuery request, CancellationToken cancellationToken)
    {
        Option? answer = await _context.Answers.FindAsync(request.AnswerId);

        if (answer == null)
        {
            return Result.Failure("Answer not found.");
        }

        return Result.Success<GetAnswerByIdQuery>("Answer details", answer);
    }
}
