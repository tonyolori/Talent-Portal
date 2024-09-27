using Application.Common.Models;
using Application.Interfaces;
using Domain.Enum;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Instructors.Commands;

public class CreateInstructorCommand:IRequest<Result>
{
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; } 
    
    public string AssignedCourse { get; set; }
    
    
    public class CreateInstructorCommandHandler(IApplicationDbContext context, UserManager<Instructor> userManager,
        RoleManager<IdentityRole> roleManager) : IRequestHandler<CreateInstructorCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly UserManager<Instructor> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        

        public async Task<Result> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
        {

            Instructor? instructorExist = await _userManager.FindByEmailAsync(request.Email);
            if (instructorExist != null)
                return Result.Failure(request, "Instructor already exists");
            
        Instructor instructor = new ()
            {
                UserName = request.Name,
                Email = request.Email,
                AssignedCourse = request.AssignedCourse,
                Role = UserRoles.Instructor,
                RoleDesc = UserRoles.Instructor.ToString(),
                Status = Status.Active,
                StatusDes = Status.Active.ToString(),
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow
            };

            IdentityResult result = await _userManager.CreateAsync(instructor, request.Password); 
            if (!result.Succeeded)
            {
                string errors = string.Join("\n", result.Errors.Select(e => e.Description));
                return Result.Failure("Instructor creation failed!\n" + errors);
            }

            const string roleName = "Admin";
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                await _userManager.AddToRoleAsync(instructor, roleName);
            }

            return Result.Success<CreateInstructorCommand>("Instructor created successfully.", instructor);
        }
    }
}