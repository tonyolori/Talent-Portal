using Domain.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;


namespace Application.Dto;
public class StudentTaskDetailsDto(ModuleTask task, SubmissionDetails details)
{

    // Fields from ModuleTask
    public string Title { get; set; } = task.Title;
    public string Description { get; set; } = task.Description;
    public string Instructions { get; set; } = task.Instructions;
    public DateTime CreatedDate { get; set; } = task.CreatedDate;
    public DateTime DueDate { get; set; } = task.DueDate;
    [ForeignKey(nameof(ProgrammeId))]
    public int ProgrammeId { get; set; } = task.ProgrammeId;

    public int Week { get; set; } = task.Week;

    // Fields from SubmissionDetails
    public float? Grade { get; set; } = details.Grade;
    public string? SubmissionLink { get; set; } = details.SubmissionLink;
    public DateTime? SubmissionDate { get; set; } = details.SubmissionDate;
    public string? FacilitatorFeedBack { get; set; } = details.FacilitatorFeedBack;
    public ModuleTaskStatus TaskStatus { get; set; } = details.TaskStatus;
    public string? TaskStatusDesc { get; set; } = details.TaskStatusDesc;

    // Additional field for joining (assuming StudentId is unique identifier)
    [ForeignKey(nameof(StudentId))]
    public string StudentId { get; set; } = details.StudentId;
}