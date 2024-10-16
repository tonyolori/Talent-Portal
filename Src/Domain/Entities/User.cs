﻿using Domain.Common.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Domain.Entities;
public class User : BaseUser
{
    
    public bool IsVerified { get; set; }

    public int ProgrammeId { get; set; }

    [ForeignKey(nameof(ProgrammeId))]
    
    public Programme Programme { get; set; }
    
    public string AssignedCourse { get; set; }
    
    public PaymentType PaymentType { get; set; }
    
    public string PaymentTypeDes { get; set; }
    
    public virtual ICollection<ModuleTask> AssignedTasks { get; set; }

    public virtual ICollection<Quiz> Quizzes { get; set; }
    
    public virtual ICollection<Module> Modules { get; set; } 
}

