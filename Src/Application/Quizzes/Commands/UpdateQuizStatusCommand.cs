using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.Quizzes.Commands
{
    public class UpdateQuizStatusCommand : IRequest<Result>
    {
        public int QuizId { get; set; }
        
        public QuizStatus NewStatus { get; set; }
    }

    public class UpdateQuizStatusCommandHandler : IRequestHandler<UpdateQuizStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdateQuizStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateQuizStatusCommand request, CancellationToken cancellationToken)
        {
            Quiz? quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken);

            if (quiz == null)
            {
                return Result.Failure($"Quiz with ID {request.QuizId} not found.");
            }
            
            
           

            quiz.QuizStatus = request.NewStatus;
            quiz.QuizStatusDes = request.NewStatus.ToString();

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Quiz status updated successfully.");
        }
    }
}