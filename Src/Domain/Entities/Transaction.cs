using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Entities;
using Domain.Enum;

namespace Domain.Entities;

public class Transaction:BaseEntity
{
    public string TransactionReference { get; set; }
    public string Email { get; set; }
    public string PreferredProgramme { get; set; }
    public EducationalLevel EducationalLevel { get; set; }
    public EmploymentStatus EmploymentStatus { get; set; }
    public ApplicationType ApplicationType { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; }
    public decimal Amount { get; set; }
    
    public string TransactionStatus { get; set; }
    
    public string StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    
    public Student Student { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
