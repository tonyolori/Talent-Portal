using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;

namespace Application.Instructors.Commands;
public class DeactivateInstructorCommand : IRequest<Result>
{
    public int InstructorId { get; set; }

    public class DeactivateInstructorCommandHandler(IApplicationDbContext context) : IRequestHandler<DeactivateInstructorCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
 

        public async Task<Result> Handle(DeactivateInstructorCommand request, CancellationToken cancellationToken)
        {
            Instructor? instructor = await _context.Instructors.FindAsync(request.InstructorId);

            if (instructor == null)
            {
                return Result.Failure("Instructor not found.");
            }

            instructor.Status = Status.Inactive;
            instructor.StatusDes = "Inactive";

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<DeactivateInstructorCommand>("Instructor deactivated successfully.", instructor);
        }
    }
}