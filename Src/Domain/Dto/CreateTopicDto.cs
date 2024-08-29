using Domain.Common.Enum;

namespace Domain.Dto;

public class CreateTopicDto
{
    public string Title { get; set; }
    public string MainContent { get; set; }
    public string? SubContent { get; set; }
    public TopicStatus Status { get; set; } = TopicStatus.NotCompleted; // Default status
}