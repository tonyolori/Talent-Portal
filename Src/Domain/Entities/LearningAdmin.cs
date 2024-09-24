using Domain.Common.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Domain.Entities;
public class LearningAdmin : BaseUser
{
    public DateTime EnrollmentDate { get; set; }
    
}