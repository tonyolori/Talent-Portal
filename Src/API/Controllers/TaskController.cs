using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Tasks.Queries;
using Application.Tasks.Commands;


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
        return Ok(await _mediator.Send(new GetTaskByIdQuery{TaskId = id}));
    }
    [HttpGet("student/{id}")]
    public async Task<IActionResult> GetAssignedTasks(string id)
    {
        return Ok(await _mediator.Send(new GetAssignedTasksQuery { StudentId = id}));
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
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitTask(SubmitTaskCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    [HttpPost("unsubmit")]

    public async Task<IActionResult> UnsubmitTask(UnsubmitTaskCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
        
    [HttpGet("instructor/{id}")]
    public async Task<IActionResult> GetAllTaskByInstructor(string id)
    {
        return Ok(await _mediator.Send(new GetAllTaskByInstructorIdQuery{InstructorId = id}));
    }
    
    [HttpGet("instructor/assigned/{id}")]
    public async Task<IActionResult> GetAllAssignedTask(string id)
    {
        return Ok(await _mediator.Send(new GetAllAssignedTaskByInstructorIdQuery{InstructorId = id}));
    }
    
        
    [HttpGet("instructor/unassigned/{id}")]
    public async Task<IActionResult> GetAllUnAssignedTask(string id)
    {
        return Ok(await _mediator.Send(new GetAllUnAssignedTaskByInstructorIdQuery(){InstructorId = id}));
    }
}