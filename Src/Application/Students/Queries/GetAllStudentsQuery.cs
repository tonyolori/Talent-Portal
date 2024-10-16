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
        private readonly UserManager<User> _userManager;

        public GetAllStudentsQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            List<User>? students = await _userManager.Users.ToListAsync(cancellationToken);

            if (students == null || !students.Any())
            {
                return Result.Failure("No students found.");
            }

            var response = new  
            {  
                students , 
                studentsLength = students.Count,  
            };  
            return Result.Success<GetAllStudentsQuery>("All students", response);
        }
    }
}