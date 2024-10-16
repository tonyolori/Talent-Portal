using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Application.Instructors.Commands;
public class DeactivateInstructorCommand : IRequest<Result>
{
    public string InstructorId { get; set; }

    public class DeactivateInstructorCommandHandler : IRequestHandler<DeactivateInstructorCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IApplicationDbContext _context;

        public DeactivateInstructorCommandHandler(UserManager<User> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result> Handle(DeactivateInstructorCommand request, CancellationToken cancellationToken)
        {
            User? instructor = await _userManager.FindByIdAsync(request.InstructorId);

            if (instructor == null || instructor.UserType != UserType.Instructor)
            {
                return Result.Failure("Instructor not found or is not of type Instructor.");
            }

            instructor.UserStatus = Status.Inactive;
            instructor.UserStatusDes = Status.Inactive.ToString();

            IdentityResult updateResult = await _userManager.UpdateAsync(instructor);
            if (!updateResult.Succeeded)
            {
                string errors = string.Join("\n", updateResult.Errors.Select(e => e.Description));
                return Result.Failure("Deactivation failed: " + errors);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Instructor deactivated successfully.");
        }
    }
}