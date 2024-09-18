using Application.Modules.Commands;
using Application.Modules.Queries;
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
    public async Task<IActionResult> CreateModule(string studentId)
    {
        GetAssignedTasksQuery query = new() { StudentId = studentId};

        return Ok(await _mediator.Send(query));
    }

    [HttpGet("Curriculum")]
    public async Task<IActionResult> GetCurriculum(int programmeId)
    {
        return Ok(await _mediator.Send(new GetModulesInProgrammeQuery { Id = programmeId }));
    }

    //[HttpGet("all")]
    //public async Task<IActionResult> GetAllModule()
    //{
    //    return Ok(await _mediator.Send(new GetAllModulesQuery() { }));
    //}



}