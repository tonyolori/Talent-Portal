using Application.Common.Helpers;
using Application.Common.Models;
using Application.Extensions;
using Application.Interfaces;
using Application.LearningAdmins.Commands;
using Domain.Common.Enum;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Application.Admins.Commands;

public class RegisterLearningAdminCommand : IRequest<Result>
{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
   
}

public class RegisterAdminCommandHandler(
    IEmailService emailSender,
    UserManager<LearningAdmin> userManager,
    RoleManager<IdentityRole> roleManager,
    IConnectionMultiplexer redis)
    : IRequestHandler<RegisterLearningAdminCommand, Result>
{
    private readonly IEmailService _emailSender = emailSender;
    private readonly UserManager<LearningAdmin> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IDatabase _redisDb = redis.GetDatabase();

   
    public async Task<Result> Handle(RegisterLearningAdminCommand request, CancellationToken cancellationToken)
    {
        
        await request.ValidateAsync(new LearningAdminCreateValidator(), cancellationToken);
        
        LearningAdmin? adminExist = await _userManager.FindByEmailAsync(request.Email);
        if (adminExist != null)
            return Result.Failure(request, "Learning Admin already exists");

        LearningAdmin learningAdmin = new()
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = UserRoles.Admin, 
            RoleDesc = UserRoles.Admin.ToString(),  
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        IdentityResult result = await _userManager.CreateAsync(learningAdmin, request.Password); 
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
            await _userManager.AddToRoleAsync(learningAdmin, roleName);
        }
        
        return Result.Success<RegisterLearningAdminCommand>("Learning Admin successfully.", learningAdmin);
    }

  
}