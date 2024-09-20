using Application.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Auth.Commands;
using Application.Tasks.Queries;
using Application.Programmes.Commands;
using Application.Quizzes.Commands;
using Application.Quizzes.Queries;


namespace API.Controllers;

[ApiController]
[Route("api/quiz")]
public class QuizController(IMediator mediator) : Controller
{

    private readonly IMediator _mediator = mediator;

    [HttpPost("create")]
    public async Task<IActionResult> CreateQuiz(CreateQuizCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpPost("student/update-quiz-status")]
    public async Task<IActionResult> UpdateQuizStatus(UpdateQuizStatusCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpPost("summary/{studentId}")]
    public async Task<IActionResult> GetQuizStatusSummary(string studentId)
    {
        return Ok(await _mediator.Send(new GetQuizStatusSummaryByStudentIdQuery{StudentId = studentId}));
    }

}