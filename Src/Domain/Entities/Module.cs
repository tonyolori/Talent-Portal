namespace Domain.Entities;

public class Module
{
    public int Id { get; set; }

    public Guid Guid { get; set; } = Guid.NewGuid();

    public string Title { get; set; }
        
    public string ModuleImageUrl { get; set; }
        
    // Navigation property for the related topics
    public ICollection<Topic> Topics { get; set; } = new List<Topic>();

    public string Description { get; set; }

    public string Objectives { get; set; }

    public string FacilitatorName { get; set; }

    public string FacilitatorId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
        
    public string Timeframe { get; set; }
        
    public string Progress { get; set; }
        
    public string AdditionalResources { get; set; }
}