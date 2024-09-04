using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Quizzes.Commands
{
    public class CreateQuizCommand : IRequest<Result>
    {
        public string Title { get; set; }
    }

    public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateQuizCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            Quiz newQuiz = new()
            {
                Title = request.Title
            };

            _context.Quizzes.AddAsync(newQuiz);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<CreateQuizCommand>("Quiz created successfully!", newQuiz);
        }
    }
}