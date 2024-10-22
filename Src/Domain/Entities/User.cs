using Domain.Common.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Domain.Entities;
public class User : BaseUser
{
    public bool IsVerified { get; set; }
    
    // ProgrammeId is only needed for Student users
    public int? ProgrammeId { get; set; }  // Made nullable to accommodate Admin and Instructor

    [ForeignKey(nameof(ProgrammeId))]
    public Programme Programme { get; set; }
    
    public string AssignedCourse { get; set; }
    
    public PaymentType PaymentType { get; set; }
    
    public string PaymentTypeDes { get; set; }
    
    public virtual ICollection<ModuleTask> AssignedTasks { get; set; }
    
    public virtual ICollection<Quiz> Quizzes { get; set; }
    
    public virtual ICollection<Module> Modules { get; set; }

    // Additional properties or methods can be added here to handle role-specific logic if necessary
}