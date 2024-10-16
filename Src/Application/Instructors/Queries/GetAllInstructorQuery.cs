using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetAllInstructorsQuery : IRequest<Result>
{
    public class GetAllInstructorsQueryHandler : IRequestHandler<GetAllInstructorsQuery, Result>
    {
        private readonly UserManager<User> _userManager;

        public GetAllInstructorsQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(GetAllInstructorsQuery request, CancellationToken cancellationToken)
        {
            // Get all users of type Instructor
            List<User> instructors = await _userManager.Users
                .Where(u => u.UserType == UserType.Instructor)
                .ToListAsync(cancellationToken);

            if (!instructors.Any())
            {
                return Result.Failure("No instructors found.");
            }

            var response = new
            {
                Instructors = instructors,
                InstructorsLength = instructors.Count
            };

            return Result.Success("All Instructors", response);
        }
    }
}