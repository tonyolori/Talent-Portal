using Application.Dashboard.Queries;
using Application.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ApiController]
[Route("api/Dashboard")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("recent-tasks/{studentId}")]
    public async Task<IActionResult> recentTasks(string studentId)
    {
        //this is on purpose, no need to create a new query
        GetAssignedTasksQuery query = new() { StudentId = studentId};

        return Ok(await _mediator.Send(query));
    }

    [HttpGet("curriculum")]
    public async Task<IActionResult> GetCurriculum(int programmeId)
    {
        return Ok(await _mediator.Send(new GetModulesInProgrammeQuery { Id = programmeId }));
    }

    [HttpGet("progress-summary")]
    public async Task<IActionResult> GetProgressSummary(string studentId)
    {
        return Ok(await _mediator.Send(new GetProgressSummaryQuery { StudentId = studentId }));
    }
    //[HttpGet("all")]
    //public async Task<IActionResult> GetAllModule()
    //{
    //    return Ok(await _mediator.Send(new GetAllModulesQuery() { }));
    //}



}