using Domain.Common.Entities;

namespace Application.Dto
{
    public class CalendarSlotDto : BaseEntity
    {
        public string ClassTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
