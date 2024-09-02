using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class Grade
{
    [Key]
    public Guid TaskId { get; set; }
    
    public required float Value { get; set; }

}
