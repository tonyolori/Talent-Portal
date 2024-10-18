using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Web.WebPages;

namespace Application.Calendar.Commands
{
    public class AddCourseToCalendarCommand : IRequest<Result>
    {
        public required string ClassTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ProgrammeId { get; set; }
    }

    public class AddCourseToCalendarCommandHandler : IRequestHandler<AddCourseToCalendarCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public AddCourseToCalendarCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AddCourseToCalendarCommand request, CancellationToken cancellationToken)
        {
            if (request.ClassTitle.IsEmpty())
            {
                return Result.Failure("Class Title cannot be empty");
            }

            if (request.StartTime >= request.EndTime)
            {
                return Result.Failure("Start time must be earlier than end time.");
            }
            //TODO: add foreign key validation

            // Check for overlapping slots
            List<CalendarSlot> overlappingSlots = await _context.CalendarSlots
                .Where(s => s.StartTime <= request.EndTime && s.EndTime >= request.StartTime)
                .ToListAsync(cancellationToken);

            if (overlappingSlots.Any())
            {
                return Result.Failure("Time slot already taken.");
            }

            CalendarSlot slot = new ()
            {
                ClassTitle = request.ClassTitle,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ProgrammeId = request.ProgrammeId,
            };

            await _context.CalendarSlots.AddAsync(slot, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<AddCourseToCalendarCommand>("Slot added successfully!", slot);
        }
    }
}
