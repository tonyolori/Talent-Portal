using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;

namespace Application.Instructors.Commands;
public class ReactivateInstructorCommand : IRequest<Result>
{
    public int InstructorId { get; set; }

    public class ReactivateInstructorCommandHandler(IApplicationDbContext context) : IRequestHandler<ReactivateInstructorCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        

        public async Task<Result> Handle(ReactivateInstructorCommand request, CancellationToken cancellationToken)
        {
            Instructor? instructor = await _context.Instructors.FindAsync(request.InstructorId);

            if (instructor == null)
            {
                return Result.Failure("Instructor not found.");
            }

            instructor.Status = Status.Active;
            instructor.StatusDes = "Active";

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<ReactivateInstructorCommand>("Instructor reactivated successfully.", instructor);
        }
    }
}