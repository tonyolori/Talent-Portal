using Application.Common.Models;
using Application.Interfaces;
using Domain.Enum;
using Domain.Entities;
using MediatR;

namespace Application.Instructors.Commands;

public class CreateInstructorCommand:IRequest<Result>
{
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string AssignedCourse { get; set; }
    
    public string Role { get; set; }
    
    
    public class CreateInstructorCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateInstructorCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        

        public async Task<Result> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
        {
            
        Instructor instructor = new ()
            {
                Name = request.Name,
                Email = request.Email,
                AssignedCourse = request.AssignedCourse,
                Role = request.Role,
                Status = Status.Active,
                StatusDes = "Active",
                DateAdded = DateTime.UtcNow
            };

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<CreateInstructorCommand>("Instructor created successfully.", instructor);
        }
    }
}