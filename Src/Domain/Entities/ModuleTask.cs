
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Entities;

namespace Domain.Entities;

public class ModuleTask: BaseEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Instructions { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public required DateTime DueDate { get; set; }
    public int Week { get; set; }
    
    [ForeignKey(nameof(ProgrammeId))]
    public int ProgrammeId { get; set; }
    
    [ForeignKey(nameof(InstructorId))]
    public string InstructorId { get; set; }
}
