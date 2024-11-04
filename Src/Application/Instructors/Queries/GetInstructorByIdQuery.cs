using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enum;

public class GetInstructorByIdQuery : IRequest<Result>
{
    
    
    [Newtonsoft.Json.JsonIgnore]  
    [JsonIgnore]  
    public string InstructorId { get; set; } 

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

            return Result.Success<GetInstructorByIdQuery>("Instructor details", instructor);
        }
    }
}