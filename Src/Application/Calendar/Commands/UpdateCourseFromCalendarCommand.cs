using Application.Common.Models;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Web.WebPages;


namespace Application.Calendar.Commands
{
    public class UpdateCourseFromCalendarCommand : IRequest<Result>
    {
        public string ClassTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Id { get; set; }
    }

    public class UpdateCourseFromCalendarCommandHandler : IRequestHandler<UpdateCourseFromCalendarCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCourseFromCalendarCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateCourseFromCalendarCommand request, CancellationToken cancellationToken)
        {
            if (request.ClassTitle.IsEmpty())
            {
                return Result.Failure("Class Title cannot be empty");
            }

            if (request.StartTime >= request.EndTime)
            {
                return Result.Failure("Start time must be earlier than end time.");
            }

            Programme? existingProgramme = await _context.Programmes
                                            .AsNoTracking()
                                             .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (existingProgramme == null)
            {
                return Result.Failure("Invalid Slot ID");
            }

            // Check for existing slot
            CalendarSlot? existingSlot = await _context.CalendarSlots
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StartTime <= request.EndTime && s.EndTime >= request.StartTime);

            if (existingSlot == null)
            {
                return Result.Failure("Time slot Dosent Exist");
            }



            existingSlot.ClassTitle = request.ClassTitle;
            existingSlot.StartTime = request.StartTime;
            existingSlot.EndTime = request.EndTime;
            

            _context.CalendarSlots.Update(existingSlot);
            await _context.SaveChangesAsync(cancellationToken);

            CalendarSlotDto slotDto = _mapper.Map<CalendarSlotDto>(existingSlot);
            return Result.Success<UpdateCourseFromCalendarCommand>("Slot added successfully!", slotDto);
        }
    }
}
