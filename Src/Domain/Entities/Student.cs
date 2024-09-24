using Domain.Common.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Domain.Entities;
public class Student : BaseUser
{
    
    public Status UserStatus { get; set; }
    public string UserStatusDes { get; set; }
    public bool IsVerified { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public int ProgrammeId { get; set; }

    [ForeignKey(nameof(ProgrammeId))]
    
    public Programme Programme { get; set; }
    
    public ApplicationType ApplicationType { get; set; }
    
    public string PaymentTypeDes { get; set; }
    
    public virtual ICollection<ModuleTask> AssignedTasks { get; set; }

    public virtual ICollection<Quiz> Quizzes { get; set; }
    
    public virtual ICollection<Module> Modules { get; set; } 
}

