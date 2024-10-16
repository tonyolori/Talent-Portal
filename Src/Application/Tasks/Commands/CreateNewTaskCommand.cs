using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries
{
    public class CreateNewTaskCommand : IRequest<Result>
    {
        public string Title { get; set; }
        public  string Description { get; set; }
        public  string Instructions { get; set; }
        public int ModuleId { get; set; }
        public string InstructorId { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class CreateNewTaskCommandHandler : IRequestHandler<CreateNewTaskCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IApplicationDbContext _context;

        public CreateNewTaskCommandHandler(UserManager<User> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result> Handle(CreateNewTaskCommand request, CancellationToken cancellationToken)
        {
            // Retrieve instructor using UserManager
            User? instructor = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == request.InstructorId && u.UserType == UserType.Instructor, cancellationToken);

            if (instructor == null)
            {
                return Result.Failure($"Instructor with ID {request.InstructorId} not found.");
            }

            // Check if the module exists
            bool moduleExists = await _context.Modules.AnyAsync(m => m.Id == request.ModuleId, cancellationToken);
            if (!moduleExists)
            {
                return Result.Failure($"Module with ID {request.ModuleId} does not exist.");
            }

            //TODO: update the task command to reflect the fact that tasks are linked to programmes
            ModuleTask moduleTask = new()
            {
                InstructorId = instructor.Id,
                Title = request.Title,
                Description = request.Description,
                Instructions = request.Instructions,
                DueDate = request.DueDate,
            };

            await _context.Tasks.AddAsync(moduleTask, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<CreateNewTaskCommand>("Task created successfully!", moduleTask);
        }
    }
}
