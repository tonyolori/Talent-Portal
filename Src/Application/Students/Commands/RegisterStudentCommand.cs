using Application.Common.Models;
using Application.Extensions;
using Application.Interfaces;
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
        public DateTime DateOfBirth { get; set; }
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
        
        var studentExists = await _userManager.FindByEmailAsync(request.Email);
        if (studentExists != null)
            return Result.Failure(request, "Student already exists");

        var student = new Student
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
            Programme = request.Programme,
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        IdentityResult result = await _userManager.CreateAsync(student, request.Password); 
        if (!result.Succeeded)
        {
            var errors = string.Join("\n", result.Errors.Select(e => e.Description));
            return Result.Failure("Student creation failed!\n" + errors);
        }

        var roleName = request.Role; 
        if (!await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            await _userManager.AddToRoleAsync(student, roleName);
        }

        await _emailSender.SendEmailAsync(request.Email, "Welcome", $"Welcome {request.FirstName}!");
        return Result.Success<RegisterStudentCommand>( "Student registered successfully!", result );
    }
}
