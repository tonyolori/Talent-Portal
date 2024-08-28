namespace Domain.Entities;

public class Module
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; }
    
    public string ModuleImageUrl { get; set; }
    
    public List<string> Topics { get; set; } = new();

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