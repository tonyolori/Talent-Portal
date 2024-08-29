using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Tasks.Queries;


namespace API.Controllers;

[ApiController]
[Route("api/Tasks")]
public class TaskController(IMediator mediator) : Controller
{

    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    public async Task<IActionResult> GetAllTasks()
    {
        return Ok(await _mediator.Send(new GetAllTasksQuery { } ));
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetTask(GetTaskByIdQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpPost("add/")]
    public async Task<IActionResult> AddTask(CreateNewTaskCommand Command)
    {
        return Ok(await _mediator.Send(Command));
    }

}