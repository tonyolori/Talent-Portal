using Domain.Common.Entities;
using Domain.Enum;

namespace Domain.Entities;
public class Student : BaseUser
{
    public Programme Programme { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public ICollection<ModuleTask> AssignedTasks { get; set; } // Many-to-many relationship with Task (navigation property)

}

