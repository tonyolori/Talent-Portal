using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Instructors.Commands;
public class UpdateInstructorCommand : IRequest<Result>
{
    public int InstructorId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string AssignedCourse { get; set; }
    public string Role { get; set; }

    public class UpdateInstructorCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateInstructorCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;

    
        public async Task<Result> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
        {
            Instructor? instructor = await _context.Instructors.FindAsync(request.InstructorId);

            if (instructor == null)
            {
                return Result.Failure("Instructor not found.");
            }

            instructor.Name = request.Name;
            instructor.Email = request.Email;
            instructor.AssignedCourse = request.AssignedCourse;
            instructor.Role = request.Role;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<UpdateInstructorCommand>("Instructor updated successfully.", instructor);
        }
    }
}
