using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Entities;
using Domain.Enum;

namespace Domain.Entities;

public class Quiz : BaseEntity
{
    public string? StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    
    public string InstructorId { get; set; }

    [ForeignKey(nameof(InstructorId))]

    public User User { get; set; }
    
    public int ModuleId { get; set; }

    [ForeignKey(nameof(ModuleId))]
    public Module Module { get; set; }

    public QuizStatus QuizStatus { get; set; }

    public string QuizStatusDes { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}