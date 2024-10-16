using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Application.Instructors.Commands;
public class UpdateInstructorCommand : IRequest<Result>
{
    public string InstructorId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string AssignedCourse { get; set; }

    public class UpdateInstructorCommandHandler : IRequestHandler<UpdateInstructorCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IApplicationDbContext _context;

        public UpdateInstructorCommandHandler(UserManager<User> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
        {
            // Find the instructor using UserManager by their Id
            User? instructor = await _userManager.FindByIdAsync(request.InstructorId);

            if (instructor == null || instructor.UserType != UserType.Instructor)
            {
                return Result.Failure("Instructor not found or is not of type Instructor.");
            }

            // Update instructor fields
            instructor.UserName = request.Name;
            instructor.Email = request.Email;
            instructor.AssignedCourse = request.AssignedCourse;

            IdentityResult updateResult = await _userManager.UpdateAsync(instructor);
            if (!updateResult.Succeeded)
            {
                string errors = string.Join("\n", updateResult.Errors.Select(e => e.Description));
                return Result.Failure("Instructor update failed: " + errors);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Instructor updated successfully.");
        }
    }
}
