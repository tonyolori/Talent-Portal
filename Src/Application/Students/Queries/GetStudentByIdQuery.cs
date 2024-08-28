using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Students.Queries
{
    public class GetStudentByIdQuery : IRequest<Result>
    {
        public string Id { get; set; }
        
    }

    public class GetUserByIdQueryHandler(UserManager<Student> userManager) : IRequestHandler<GetStudentByIdQuery, Result>
    {
        private readonly UserManager<Student> _userManager = userManager;

        public async Task<Result> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            Student user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                return Result.Failure<GetStudentByIdQuery>("User Id does not exist");
            }

            // Return the user or null if not found
            return Result.Success(user);
        }
    }
}