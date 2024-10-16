using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly IApplicationDbContext _context;

        public CreateQuizCommandHandler(UserManager<User> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
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

            // Ensure student exists and check for user type
            User? student = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == request.StudentId && u.UserType == UserType.Student, cancellationToken);

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
                StudentId = request.StudentId, // Ensure the student ID is set correctly
                ModuleId = request.ModuleId // Set the module ID
            };

            await _context.Quizzes.AddAsync(newQuiz, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Quiz created successfully!", newQuiz);
        }
    }
}
