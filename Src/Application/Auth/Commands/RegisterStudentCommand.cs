using Application.Common.Helpers;
using Application.Common.Models;
using Application.Extensions;
using Application.Interfaces;
using Domain.Common.Enum;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands;

public class RegisterStudentCommand : IRequest<Result>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } 
    public string Programme { get; set; } 
}

public class RegisterStudentCommandHandler : IRequestHandler<RegisterStudentCommand, Result>
{
    private readonly IEmailService _emailSender;
    private readonly UserManager<Student> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IDatabase _redisDb;
    private readonly IApplicationDbContext _context; // Add the context for database interaction

    public RegisterStudentCommandHandler(
        IEmailService emailSender,
        UserManager<Student> userManager,
        RoleManager<IdentityRole> roleManager,
        IConnectionMultiplexer redis,
        IApplicationDbContext context) // Inject the context
    {
        _emailSender = emailSender;
        _userManager = userManager;
        _roleManager = roleManager;
        _redisDb = redis.GetDatabase();
        _context = context;
    }

    public async Task<Result> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
        await request.ValidateAsync(new StudentCreateValidator(), cancellationToken);

        // Check if the student already exists
        Student? studentExist = await _userManager.FindByEmailAsync(request.Email);
        if (studentExist != null)
            return Result.Failure(request, "Student already exists");

        // Check if the programme exists in the database
        
        Programme? existingProgramme = await _context.Programmes
            .FirstOrDefaultAsync(p => p.Type == request.Programme, cancellationToken);

        if (existingProgramme == null)
        {
            return Result.Failure($"Programme '{request.Programme}' does not exist");
        }

        // Create the student entity
        Student student = new()
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EnrollmentDate = DateTime.Now,
            Role = Enum.Parse<UserRoles>("Student", true),
            RoleDesc = "Student",
            IsVerified = false,
            UserStatus = Status.Inactive,
            UserStatusDes = Status.Active.ToString(),
            ProgrammeId = existingProgramme.Id, // Save the ProgrammeId
            PaymentType = PaymentType.BootCampOnly,
            PaymentTypeDes = "BootCamp",
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        // Create the student in the UserManager
        IdentityResult result = await _userManager.CreateAsync(student, request.Password);
        if (!result.Succeeded)
        {
            string errors = string.Join("\n", result.Errors.Select(e => e.Description));
            return Result.Failure("Student creation failed!\n" + errors);
        }

        // Check if role exists, and add student to role
        string roleName = student.RoleDesc;
        if (!await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            await _userManager.AddToRoleAsync(student, roleName);
        }

        // Generate and save registration code to Redis
        string registrationCode = GenerateCode.GenerateRegistrationCode();
        await _redisDb.StringSetAsync($"RegistrationCode:{student.Email}", registrationCode, TimeSpan.FromHours(2));

        // Send the registration code to the user's email
        await _emailSender.SendEmailAsync(student.Email, "Registration Confirmation Code",
            $"Your registration confirmation code is {registrationCode}");

        return Result.Success("Registration code sent successfully! Please confirm your registration.", student);
    }
}
