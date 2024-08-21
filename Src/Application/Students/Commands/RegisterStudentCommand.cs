using Application.Common.Models;
using Application.Extensions;
using Application.Interfaces;
using Domain.Common.Enum;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Students.Commands;

public class RegisterStudentCommand : IRequest<Result>
{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public string Role { get; set; }
        public string DateOfBirth { get; set; }
        public Programme Programme { get; set; }
}

public class RegisterStudentCommandHandler(
            IEmailService emailSender,
            UserManager<Student> userManager,
            RoleManager<IdentityRole> roleManager)
 : IRequestHandler<RegisterStudentCommand, Result>
{
    private readonly IEmailService _emailSender = emailSender;
    private readonly UserManager<Student> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

   
    public async Task<Result> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        
        // Validate the request
        await request.ValidateAsync(new StudentCreateValidator(), cancellationToken);
        
        Student? Dbstudent = await _userManager.FindByEmailAsync(request.Email);
        if (Dbstudent != null)
            return Result.Failure(request, "Student already exists");

        Student student = new()
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            EnrollmentDate =  DateTime.Now,
            Role = Enum.Parse<UserRoles>(request.Role, true), 
            RoleDesc = request.Role,  
            UserStatus = Status.Active,
            UserStatusDes = Status.Active.ToString(),
            Programme = request.Programme,
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        IdentityResult result = await _userManager.CreateAsync(student, request.Password); 
        if (!result.Succeeded)
        {
            string errors = string.Join("\n", result.Errors.Select(e => e.Description));
            return Result.Failure("Student creation failed!\n" + errors);
        }

        string roleName = request.Role; 
        if (!await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            await _userManager.AddToRoleAsync(student, roleName);
        }

        await _emailSender.SendWelcomeEmailAsync(request.Email, student.FirstName);
        return Result.Success<RegisterStudentCommand>( "Student registered successfully!", student);
    }
}
