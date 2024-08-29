using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Tasks.Queries;


namespace API.Controllers;

[ApiController]
[Route("api/task")]
public class TaskController(IMediator mediator) : Controller
{

    private readonly IMediator _mediator = mediator;

    [HttpPost("create")]
    public async Task<IActionResult> AddTask(CreateNewTaskCommand Command)
    {
        return Ok(await _mediator.Send(Command));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        return Ok(await _mediator.Send(new GetTaskByIdQuery{Id = id}));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllTasks()
    {
        return Ok(await _mediator.Send(new GetAllTasksQuery { }));
    }

}