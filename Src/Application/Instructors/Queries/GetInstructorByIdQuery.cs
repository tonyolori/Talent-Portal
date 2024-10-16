using Microsoft.AspNetCore.Identity;
using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using System.Threading;
using System.Threading.Tasks;

public class GetInstructorByIdQuery : IRequest<Result>
{
    public string InstructorId { get; set; } // Change to string to match IdentityUser's Id type

    public class GetInstructorByIdQueryHandler : IRequestHandler<GetInstructorByIdQuery, Result>
    {
        private readonly UserManager<User> _userManager;

        public GetInstructorByIdQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(GetInstructorByIdQuery request, CancellationToken cancellationToken)
        {
            // Find the instructor by User Id and ensure the UserType is Instructor
            User? instructor = await _userManager.FindByIdAsync(request.InstructorId);
            
            if (instructor == null || instructor.UserType != UserType.Instructor)
            {
                return Result.Failure("Instructor not found.");
            }

            return Result.Success("Instructor details", instructor);
        }
    }
}