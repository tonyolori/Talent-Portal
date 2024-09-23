using Application.Common.Models;  
using MediatR;  
using Application.Interfaces;  
using Domain.Entities;  
using Domain.Enum;  
using Microsoft.EntityFrameworkCore;  

namespace Application.Quizzes.Commands  
{  
    public class CreateQuizCommand : IRequest<Result>  
    {  
        public string Title { get; set; }  
        public string StudentId { get; set; }  
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
            // Check if the module exists  
            bool moduleExists = await _context.Modules.AnyAsync(m => m.Id == request.ModuleId, cancellationToken);  
            if (!moduleExists)  
            {  
                return Result.Failure($"Module with ID {request.ModuleId} does not exist.");  
            }  
           
            // Ensure student exists  
            Student? student = await _context.Students  
                .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);  
            
            if (student == null)  
            {  
                return Result.Failure($"Student with ID {request.StudentId} does not exist.");  
            }  
            
            // Create new quiz  
            Quiz newQuiz = new()  
            {  
                Title = request.Title,  
                QuizStatus = QuizStatus.NotStarted,  
                QuizStatusDes = "NotStarted",  
                StudentId = request.StudentId, // Make sure to set the StudentId for the new quiz  
                ModuleId = request.ModuleId // Ensure to set the ModuleId if needed  
            };  

            await _context.Quizzes.AddAsync(newQuiz, cancellationToken); // Await the AddAsync  
            await _context.SaveChangesAsync(cancellationToken);  

            return Result.Success<CreateQuizCommand>("Quiz created successfully!", newQuiz);  
        }  
    }  
}