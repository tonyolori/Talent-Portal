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

    public class GetStudentByIdQueryHandler(UserManager<User> userManager) : IRequestHandler<GetStudentByIdQuery, Result>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<Result> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            User? student = await _userManager.FindByIdAsync(request.Id);

            if (student == null)
            {
                return Result.Failure("Student Id does not exist");
            }

            
            return Result.Success<GetStudentByIdQuery>("Student found,.", student);
        }
    }
}