using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Application.Instructors.Commands;
public class ReactivateInstructorCommand : IRequest<Result>
{
    public string InstructorId { get; set; }

    public class ReactivateInstructorCommandHandler : IRequestHandler<ReactivateInstructorCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IApplicationDbContext _context;

        public ReactivateInstructorCommandHandler(UserManager<User> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result> Handle(ReactivateInstructorCommand request, CancellationToken cancellationToken)
        {
            User? instructor = await _userManager.FindByIdAsync(request.InstructorId);

            if (instructor == null || instructor.UserType != UserType.Instructor)
            {
                return Result.Failure("Instructor not found or is not of type Instructor.");
            }

            instructor.UserStatus = Status.Active;
            instructor.UserStatusDes = Status.Active.ToString();

            IdentityResult updateResult = await _userManager.UpdateAsync(instructor);
            if (!updateResult.Succeeded)
            {
                string errors = string.Join("\n", updateResult.Errors.Select(e => e.Description));
                return Result.Failure("Reactivation failed: " + errors);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Instructor reactivated successfully.");
        }
    }
}