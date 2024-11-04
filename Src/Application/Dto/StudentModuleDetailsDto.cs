using Domain.Entities;
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Dto;

public class StudentModuleDetailsDto(Module module, StudentModule? progress)
{
    // Fields from Module
    public string Title { get; set; } = module.Title;

    public string ModuleImageUrl { get; set; } = module.ModuleImageUrl;

    public string Description { get; set; } = module.Description;

    public string Objectives { get; set; } = module.Objectives;

    public string FacilitatorName { get; set; } = module.Objectives;

    public string FacilitatorId { get; set; } = module.InstructorId;

    public ModuleStatus ModuleStatus { get; set; } = module.ModuleStatus;
    public string ModuleStatusDes { get; set; } = module.ModuleStatusDes;
    public int Timeframe { get; set; } = module.Timeframe;


    // Fields from StudentModule progress table

    public DateTime? EnrollmentDate { get; set; } = progress?.EnrollmentDate;

    public DateTime? EndDate { get; set; } = progress?.EndDate;

    public StudentModuleProgress? StudentModuleProgress { get; set; } = progress?.StudentModuleProgress;

    public string? StudentModuleProgressDes { get; set; } = progress?.StudentModuleProgressDes;


    // Additional field for joining (assuming StudentId is unique identifier)

    [ForeignKey(nameof(ProgrammeId))]
    public int? ProgrammeId { get; set; } = module.ProgrammeId;

    [ForeignKey(nameof(StudentId))]
    public string? StudentId { get; set; } = progress?.StudentId;

    [ForeignKey(nameof(ModuleId))]
    public int? ModuleId { get; set; } = module.Id;
}
