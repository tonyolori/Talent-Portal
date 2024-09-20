using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Entities;

namespace Domain.Entities;

public class Transaction:BaseEntity
{
    public string TransactionReference { get; set; }
    public string Email { get; set; }
    public string PreferredProgramme { get; set; }
    public string EducationalLevel { get; set; }
    public string EmploymentStatus { get; set; }
    public string ApplicationType { get; set; }
    public decimal Amount { get; set; }
    
    public string TransactionStatus { get; set; }
    
    public string StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    
    public Student Student { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
