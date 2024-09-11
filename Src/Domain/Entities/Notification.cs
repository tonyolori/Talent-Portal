using Domain.Common.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace Domain.Entities;

public class Notification : BaseEntity
{
    public required string Title;
    public required string Message { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }
    [ForeignKey(nameof(StudentId))]
    public int StudentId { get; set; }
}
