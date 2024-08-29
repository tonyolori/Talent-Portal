﻿
using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Entities;

namespace Domain.Entities;

public class ModuleTask: BaseUser
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Instructions { get; set; }
    public string? SubmissionLink { get; set; }
    
    public string? FacilitatorFeedBack { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    public DateTime SubmissionDate { get; set; }
    public ModuleTaskStatus Status { get; set; }
    public int ModuleId { get; set; } // Foreign key to Module
    
    [ForeignKey(nameof(ModuleId))]
    public Module Module { get; set; } // Navigation property for one-to-many relationship
}
