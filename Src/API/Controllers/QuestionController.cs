using Application.Questions.Commands;
using Application.Questions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/question")]
public class QuestionController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    // Create a new question
    [HttpPost("create")]
    public async Task<IActionResult> CreateQuestion(CreateQuestionCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    // Get question by ID along with its answers
    [HttpGet("{questionId}")]
    public async Task<IActionResult> GetQuestionById(int questionId)
    {
        return Ok(await _mediator.Send(new GetQuestionByIdQuery { QuestionId = questionId }));
    }

    // Update question text and answers by question ID
    [HttpPut("update-question-and-answers")]
    public async Task<IActionResult> UpdateQuestionAndAnswers( UpdateQuestionAndAnswerByIdCommand command)
    {
      
        return Ok(await _mediator.Send(command));
    }

    // Update only the question by ID
    [HttpPut("update-question")]
    public async Task<IActionResult> UpdateQuestion( UpdateQuestionByIdCommand command)
    {
       
        return Ok(await _mediator.Send(command));
    }

    // Delete question and its corresponding answers by question ID
    [HttpDelete("{questionId}")]
    public async Task<IActionResult> DeleteQuestionAndAnswers(int questionId)
    {
        return Ok(await _mediator.Send(new DeleteQuestionAndAnswersByIdCommand { QuestionId = questionId }));
    }

    // Get all answers for a specific question
    [HttpGet("{questionId}/answers")]
    public async Task<IActionResult> GetAllAnswersByQuestionId(int questionId)
    {
        return Ok(await _mediator.Send(new GetAllAnswersByQuestionIdQuery { QuestionId = questionId }));
    }

    // Get a specific answer by ID
    [HttpGet("answer")]
    public async Task<IActionResult> GetAnswerById(int answerId)
    {
        return Ok(await _mediator.Send(new GetAnswerByIdQuery() { AnswerId = answerId }));
    }

    // Update a specific answer by ID
    [HttpPut("answer/update")]
    public async Task<IActionResult> UpdateAnswer( UpdateAnswerByIdCommand command)
    {
    
        return Ok(await _mediator.Send(command));
    }
}
