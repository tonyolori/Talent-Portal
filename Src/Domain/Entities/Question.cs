using Domain.Common.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Dto;

namespace Domain.Entities;

public class Question : BaseEntity
{
    public string QuestionText { get; set; }

    // Navigation property for the related options
    public ICollection<Option> Options { get; set; } = new List<Option>();

    // Foreign key for Quiz
    public int QuizId { get; set; }

    [ForeignKey(nameof(QuizId))]
    public Quiz Quiz { get; set; }
}