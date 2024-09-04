using Application.Answers.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;



namespace API.Controllers;

[ApiController]
[Route("api/answers")]
public class AnswerController(IMediator mediator) : Controller
{

    private readonly IMediator _mediator = mediator;

    [HttpPost("create")]
    public async Task<IActionResult> CreateAnswer(CreateAnswerCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    

}