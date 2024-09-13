using Application.Auth;
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

namespace Application.Admins.Commands;

public class RegisterAdminCommand : IRequest<Result>
{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
   
}

public class RegisterAdminCommandHandler(
    IEmailService emailSender,
    UserManager<Admin> userManager,
    RoleManager<IdentityRole> roleManager,
    IConnectionMultiplexer redis)
    : IRequestHandler<RegisterAdminCommand, Result>
{
    private readonly IEmailService _emailSender = emailSender;
    private readonly UserManager<Admin> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IDatabase _redisDb = redis.GetDatabase();

   
    public async Task<Result> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        
        // Validate the request
        await request.ValidateAsync(new AdminCreateValidator(), cancellationToken);
        
        Admin? adminExist = await _userManager.FindByEmailAsync(request.Email);
        if (adminExist != null)
            return Result.Failure(request, "StudentController already exists");

        Admin admin = new()
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = Enum.Parse<UserRoles>("Admin", true), 
            RoleDesc = "Admin",  
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        IdentityResult result = await _userManager.CreateAsync(admin, request.Password); 
        if (!result.Succeeded)
        {
            string errors = string.Join("\n", result.Errors.Select(e => e.Description));
            return Result.Failure("Admin creation failed!\n" + errors);
        }

        const string roleName = "Admin";
        if (!await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            await _userManager.AddToRoleAsync(admin, roleName);
        }

        // Generate and save registration code to Redis
        string registrationCode = GenerateCode.GenerateRegistrationCode();
        await _redisDb.StringSetAsync($"RegistrationCode:{admin.Email}", registrationCode, TimeSpan.FromHours(2));

        // Send the registration code to the user's email
        await _emailSender.SendEmailAsync(admin.Email, "Registration Confirmation Code",
            $"Your registration confirmation code is {registrationCode}");
        return Result.Success<RegisterAdminCommand>("Registration code sent successfully! Please confirm your registration.", admin);
    }

  
}