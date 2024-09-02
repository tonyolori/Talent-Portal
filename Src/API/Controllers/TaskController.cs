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

    [HttpPost("assign")]
    public async Task<IActionResult> AssignTask(AssignTaskToStudentCommand Command)
    {
        return Ok(await _mediator.Send(Command));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        return Ok(await _mediator.Send(new GetTaskByIdQuery{Id = id}));
    }
    [HttpGet("student/{id}")]
    public async Task<IActionResult> GetAssignedTasks(string id)
    {
        return Ok(await _mediator.Send(new GetAssignedTasksQuery { StudentGuid = id}));
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllTasks()
    {
        return Ok(await _mediator.Send(new GetAllTasksQuery { }));
    }
    [HttpGet("grade")]
    public async Task<IActionResult> GetTaskGrade(string studentId, int taskId)
    {
        return Ok(await _mediator.Send(new GetTaskGradeQuery { StudentId = studentId, ModuleTaskId = taskId }));
    }

}