using Application.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Auth.Commands;
using Application.Tasks.Queries;
using Application.Programmes.Commands;


namespace API.Controllers;

[ApiController]
[Route("api/programmes")]
public class ProgrammeController(IMediator mediator) : Controller
{

    private readonly IMediator _mediator = mediator;

    //[HttpGet("")]
    //public async Task<IActionResult> GetAllProgrammes()
    //{
    //    return Ok(await _mediator.Send(new GetAllProgrammesQuery { }));
    //}

    [HttpPost("add")]
    public async Task<IActionResult> AddProgramme(CreateProgrammeCommand command)
    {
        return Ok(await _mediator.Send(command));
    }


}