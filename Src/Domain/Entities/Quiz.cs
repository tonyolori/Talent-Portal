using Domain.Common.Entities;

namespace Domain.Entities;

public class Quiz: BaseEntity
{
    
    public string Title { get; set; }

    // Navigation property for the related questions
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}