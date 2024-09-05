using Domain.Common.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
public class SubmissionDetails : BaseEntity
{
    public float? Grade { get; set; }
    public string? SubmissionLink { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public string? FacilitatorFeedBack { get; set; }

    public ModuleTaskStatus TaskStatus { get; set; } = ModuleTaskStatus.NotSubmitted;
    public string? TaskStatusDesc { get; set; }

    [ForeignKey(nameof(StudentId))]
    public string StudentId { get; set; }

    [ForeignKey(nameof(TaskId))]
    public int TaskId { get; set; }

}
