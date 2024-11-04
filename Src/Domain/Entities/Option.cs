using Domain.Common.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Option : BaseEntity
{
    public string AnswerText { get; set; }

    public int QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public Question Question { get; set; }

    public bool IsCorrect { get; set; } 
}