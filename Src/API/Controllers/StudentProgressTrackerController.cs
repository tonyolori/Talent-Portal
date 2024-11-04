using Application.StudentProgressTracker.Queries;
using Application.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/StudentProgress")]
public class StudentProgressController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("recent-tasks/{studentId}")]
    public async Task<IActionResult> RecentTasks(string studentId)
    {
        GetAssignedTasksQuery query = new() { StudentId = studentId };

        return Ok(await _mediator.Send(query));
    }

    [HttpGet("allTasks")]
    public async Task<IActionResult> GetTaskCompletionStatus([FromQuery]GetStudentTasksQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpGet("allModules")]
    public async Task<IActionResult> GetModuleCompletionStatus([FromQuery] GetStudentModulesQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
    

}