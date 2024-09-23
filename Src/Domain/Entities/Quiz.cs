using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Entities;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
=======
using Domain.Enum;
>>>>>>> 8b3eb4a24a6c6f01739c516ff66da3d90746c805

namespace Domain.Entities;

public class Quiz: BaseEntity
{
    public string StudentId { get; set; }
    
    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; }

    public string Title { get; set; }
    
    public int ModuleId { get; set; } 
    
    [ForeignKey(nameof(ModuleId))]
    public Module Module { get; set; }
    
    public QuizStatus QuizStatus { get; set; }
    
    public string QuizStatusDes { get; set; }
    
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}