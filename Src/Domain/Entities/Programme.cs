using Domain.Common.Entities;


namespace Domain.Entities
{
    public class Programme : BaseEntity
    {
        public String Type { get; set; }
        public ICollection<CalendarSlot> CalendarSlots { get; set; } = [];
    }
}
