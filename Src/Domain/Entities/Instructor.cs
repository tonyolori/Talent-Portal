using Domain.Common.Entities;
using Domain.Enum;

namespace Domain.Entities;

public class Instructor: BaseEntity
{
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string AssignedCourse { get; set; }
    
    public string Role { get; set; }
    
    public Status Status { get; set; }
    
    public string StatusDes { get; set; }
    public DateTime DateAdded { get; set; }
}