using Domain.Common.Entities;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public class CalendarSlot : BaseEntity
    {
        public string ClassTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ForeignKey(nameof(ProgrammeId))]
        public int ProgrammeId { get; set; }
        public Programme Programme { get; set; }
    }
}
