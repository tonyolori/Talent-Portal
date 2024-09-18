using Domain.Common.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Domain.Entities;
public class Admin : BaseUser
{
    public DateTime EnrollmentDate { get; set; }
    
}