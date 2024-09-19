using MediatR;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Instructors.Queries;

public class GetInstructorByIdQuery : IRequest<Result>
{
    public int InstructorId { get; set; }

    public class GetInstructorByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetInstructorByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context = context;
        

        public async Task<Result> Handle(GetInstructorByIdQuery request, CancellationToken cancellationToken)
        {
            Instructor? instructor = await _context.Instructors.FindAsync(request.InstructorId);

            if (instructor == null)
            {
                return Result.Failure<Instructor>("Instructor not found.");
            }

            return Result.Success<GetInstructorByIdQuery>("instruct details", instructor );
        }
    }
}
