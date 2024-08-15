using Domain.Common.Entities;
using Domain.Enum;

namespace Domain.Entities;
public class Student : BaseUser
{
    public Programme Programme { get; set; }
    public DateTime EnrollmentDate { get; set; }

}

