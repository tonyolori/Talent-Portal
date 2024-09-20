using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Domain.Entities;

public class Quiz: BaseEntity
{
    
    public string Title { get; set; }
    
    public int ModuleId { get; set; } 
    
    [ForeignKey(nameof(ModuleId))]
    public Module Module { get; set; }

    // Navigation property for the related questions
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}