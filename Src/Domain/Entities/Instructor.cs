using Domain.Common.Entities;
using Domain.Enum;

namespace Domain.Entities;

public class Instructor: BaseUser
{
    public string AssignedCourse { get; set; }
    
    public Status Status { get; set; }
    
    public string StatusDes { get; set; }
}