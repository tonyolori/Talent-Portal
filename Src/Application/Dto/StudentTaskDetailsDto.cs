using Domain.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;


namespace Application.Dto;
public class StudentTaskDetailsDto(ModuleTask moduleTask, SubmissionDetails details)
{

    // Fields from ModuleTask
    public string Title { get; set; } = moduleTask.Title;
    public string Description { get; set; } = moduleTask.Description;
    public string Instructions { get; set; } = moduleTask.Instructions;
    public DateTime CreatedDate { get; set; } = moduleTask.CreatedDate;
    public DateTime DueDate { get; set; } = moduleTask.DueDate;
    [ForeignKey(nameof(ProgrammeId))]
    public int ProgrammeId { get; set; } = moduleTask.ProgrammeId;

    public int Week { get; set; } = moduleTask.Week;

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