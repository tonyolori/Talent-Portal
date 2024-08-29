using Application.Common.Models;
using Application.Interfaces;
using Application.Topics.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands
{
    public class UpdateStudentCommand : IRequest<Result>
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int ProgrammeId { get; set; }
    }

    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Result>
    {
        private readonly UserManager<Student> _userManager;

        public UpdateStudentCommandHandler(UserManager<Student> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            // Find the student by ID
            Student? student = await _userManager.FindByIdAsync(request.StudentId);
            if (student == null)
                return Result.Failure("Student not found");

            // Update the student details
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.DateOfBirth = request.DateOfBirth;
            student.ProgrammeId = request.ProgrammeId;
            student.LastModifiedDate = DateTime.UtcNow;

            IdentityResult updateResult = await _userManager.UpdateAsync(student);
            if (!updateResult.Succeeded)
            {
                string errors = string.Join("\n", updateResult.Errors.Select(e => e.Description));
                return Result.Failure("Student update failed!\n" + errors);
            }

            return Result.Success<UpdateTopicCommand>("Student updated successfully", student);
        }
    }
}