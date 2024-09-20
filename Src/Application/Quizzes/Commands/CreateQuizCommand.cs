using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Quizzes.Commands
{
    public class CreateQuizCommand : IRequest<Result>
    {
        public string Title { get; set; }
        
        public int ModuleId { get; set; } 
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
            bool moduleExists = await _context.Modules.AnyAsync(m => m.Id == request.ModuleId, cancellationToken);  
            if (!moduleExists)  
            {  
                return Result.Failure($"Module with ID {request.ModuleId} does not exist.");  
            }  
            
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