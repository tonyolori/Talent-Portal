using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands
{
    public class UpdateStudentCommand : IRequest<Result>
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Programme { get; set; }  // Changed from ProgrammeId to Programme
    }

    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IApplicationDbContext _context; 

        public UpdateStudentCommandHandler(UserManager<User> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            // Find the student by ID
            User? student = await _userManager.FindByIdAsync(request.StudentId);
            if (student == null || student.UserType != UserType.Student)
                return Result.Failure("Student not found");

            // Check if the programme exists
            Programme? existingProgramme = await _context.Programmes
                .FirstOrDefaultAsync(p => p.Type == request.Programme, cancellationToken);

            if (existingProgramme == null)
            {
                return Result.Failure($"Programme '{request.Programme}' does not exist");
            }

            // Update the student details
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.ProgrammeId = existingProgramme.Id;  // Update with found ProgrammeId
            student.LastModifiedDate = DateTime.UtcNow;

            // Attempt to update the student
            IdentityResult updateResult = await _userManager.UpdateAsync(student);
            if (!updateResult.Succeeded)
            {
                string errors = string.Join("\n", updateResult.Errors.Select(e => e.Description));
                return Result.Failure("Student update failed!\n" + errors);
            }

            return Result.Success("Student updated successfully", student);
        }
    }
}
