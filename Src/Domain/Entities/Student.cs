using Domain.Common.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Domain.Entities;
public class Student : BaseUser
{
    public DateTime EnrollmentDate { get; set; }
    public int ProgrammeId { get; set; }

    [ForeignKey(nameof(ProgrammeId))]
    
    public Programme Programme { get; set; }
    
    public virtual ICollection<ModuleTask> AssignedTasks { get; set; }

    //public ICollection<int>? ModuleId { get; set; }

    //[ForeignKey(nameof(ModuleId))]
    public virtual ICollection<Module> Modules { get; set; } 
}

