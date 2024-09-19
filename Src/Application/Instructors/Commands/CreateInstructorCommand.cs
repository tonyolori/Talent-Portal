using Application.Common.Models;
using Domain.Enum;
using MediatR;

namespace Application.Instructor.Commands;

public class CreateInstructorCommand:IRequest<Result>
{
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string AssignedCourse { get; set; }
    
    public InstructorRoles Role { get; set; }
}