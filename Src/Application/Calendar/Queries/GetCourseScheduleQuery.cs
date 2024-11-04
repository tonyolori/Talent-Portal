using Application.Common.Models;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Application.Calendar.Queries
{
    public class GetCourseScheduleQuery : IRequest<Result>
    {
        public int ProgrammeId { get; set; }
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string UserId { get; set; }
    }

    public class GetCourseScheduleQueryHandler : IRequestHandler<GetCourseScheduleQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCourseScheduleQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetCourseScheduleQuery request, CancellationToken cancellationToken)
        {
            List<CalendarSlot> Slots = await _context.CalendarSlots
                .Where(s => s.ProgrammeId == request.ProgrammeId)
                .ToListAsync();

            List<CalendarSlotDto> slotDtos = [];

            foreach (var slot in Slots)
            {
                slotDtos.Add(_mapper.Map<CalendarSlotDto>(slot));
            }

            return Result.Success<GetCourseScheduleQuery>("Schedule retrieved successfully!", slotDtos);
        }
    }

}
