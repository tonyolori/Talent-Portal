using Domain.Common.Entities;

namespace Domain.Entities;

public class Teacher : BaseUser
{
    public string Courses { get; set; }
}
