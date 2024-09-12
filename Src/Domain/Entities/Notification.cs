using Domain.Common.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace Domain.Entities;

public class Notification : BaseEntity
{
    public required string Title;
    public required string ShortMessage { get; set; }
    public required string  LongMessage { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }
    [ForeignKey(nameof(StudentId))]
    public string StudentId { get; set; } = string.Empty;

    [ForeignKey(nameof(TaskId))]
    public int? TaskId { get; set; }

    [ForeignKey(nameof(ModuleId))]
    public int? ModuleId { get; set; }


}
