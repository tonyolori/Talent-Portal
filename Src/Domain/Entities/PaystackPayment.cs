namespace Domain.Entities;

public class PaystackPayment
{
    public string PreferredProgramme { get; set; }
    
    public string EducationalLevel { get; set; }
    
    public string EmploymentStatus { get; set; }
    
    public string ApplicationType { get; set; }
    
    public decimal Amount { get; set; } 
    
    public string Email { get; set; }   
}