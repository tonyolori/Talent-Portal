using Domain.Common.Entities;
using Domain.Entities;

namespace Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

public class Answer: BaseEntity
{
  
    public string AnswerText { get; set; }

    public int QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public Question Question { get; set; }
}