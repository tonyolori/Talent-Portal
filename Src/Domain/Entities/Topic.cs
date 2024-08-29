using Domain.Common.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Entities;

namespace Domain.Entities;

public class Topic: BaseEntity

{
    public string Title { get; set; }
    
    public string MainContent { get; set; }
    
    public string? SubContent { get; set; }
        
    public TopicStatus Status { get; set; }

   
    public int ModuleId { get; set; }
    
    // Foreign key for Module
    [ForeignKey( nameof(ModuleId))]
    public Module Module { get; set; }
}