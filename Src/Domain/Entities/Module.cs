using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Module
{
    public int Id { get; set; }
    public Guid GuId { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    
    public required string ModuleImageUrl { get; set; }
    
    public List<string> Topics { get; set; } = new();

    public required string Description { get; set; }

    public required string Objectives { get; set; }

    public required string FacilitatorName { get; set; }

    public required string FacilitatorId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    
    public required string Timeframe { get; set; }
    public int ProgrammeId { get; set; }

    [ForeignKey(nameof(ProgrammeId))]
    public Programme Programme { get; set; }
    public virtual ICollection<ModuleTask> ModuleTasks { get; set; }
    public string Progress { get; set; }
    
    public string? AdditionalResources { get; set; }
}