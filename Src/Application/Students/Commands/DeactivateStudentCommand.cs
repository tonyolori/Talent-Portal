using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands
{
    public class DeactivateStudentCommand : IRequest<Result>
    {
        public string StudentId { get; set; }
    }

    public class DeactivateStudentCommandHandler : IRequestHandler<DeactivateStudentCommand, Result>
    {
        private readonly UserManager<Student> _userManager;

        public DeactivateStudentCommandHandler(UserManager<Student> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(DeactivateStudentCommand request, CancellationToken cancellationToken)
        {
            // Find the student by ID
            Student? student = await _userManager.FindByIdAsync(request.StudentId);
            if (student == null)
                return Result.Failure("Student not found");

            // Set the student status to Inactive
            student.UserStatus = Status.Inactive;
            student.UserStatusDes = Status.Inactive.ToString();
            student.LastModifiedDate = DateTime.UtcNow;

            IdentityResult updateResult = await _userManager.UpdateAsync(student);
            if (!updateResult.Succeeded)
            {
                string errors = string.Join("\n", updateResult.Errors.Select(e => e.Description));
                return Result.Failure("Student deactivation failed!\n" + errors);
            }

            return Result.Success<DeactivateStudentCommand>("Student deactivated successfully", student);
        }
    }
}