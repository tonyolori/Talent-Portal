using Application.Common.Models;
using MediatR;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetAllStudentsQuery : IRequest<Result>
    {
    }

    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, Result>
    {
        private readonly UserManager<Student> _userManager;

        public GetAllStudentsQueryHandler(UserManager<Student> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            // Fetch all students
            List<Student>? students = await _userManager.Users.ToListAsync(cancellationToken);

            if (students == null || !students.Any())
            {
                return Result.Failure("No students found.");
            }

            return Result.Success(students);
        }
    }
}