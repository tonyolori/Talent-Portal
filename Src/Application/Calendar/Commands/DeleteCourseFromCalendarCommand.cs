using Application.Common.Models;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Web.WebPages;

namespace Application.Calendar.Commands
{
    public class DeleteCourseFromCalendarCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteCourseFromCalendarCommandHandler : IRequestHandler<DeleteCourseFromCalendarCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteCourseFromCalendarCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteCourseFromCalendarCommand request, CancellationToken cancellationToken)
        {

            CalendarSlot? existingCalendarSlot = await _context.CalendarSlots
                                             .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (existingCalendarSlot == null)
            {
                return Result.Failure("Invalid Calendar Slot");
            }


            _context.CalendarSlots.Remove(existingCalendarSlot);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<DeleteCourseFromCalendarCommand>("Slot Removed successfully!");
        }
    }
}
