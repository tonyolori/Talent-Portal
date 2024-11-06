using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CalendarSlot, CalendarSlotDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid))
            .ForMember(dest => dest.ClassTitle, opt => opt.MapFrom(src => src.ClassTitle))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ReverseMap();

            CreateMap<Programme, ProgrammeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ReverseMap();


        }
    }
}
