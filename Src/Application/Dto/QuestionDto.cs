namespace Application.Dto;

public class QuestionDto

{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    
    public string CorrectOptionText { get; set; }
    public List<OptionDto> Options { get; set; }
}

public class OptionDto
{
    public int Id { get; set; }
    public string OptionText { get; set; }
}