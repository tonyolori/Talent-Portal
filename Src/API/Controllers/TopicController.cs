using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Tasks.Queries;
using Application.Topics.Commands;
using Application.Topics.Queries;
using Domain.Common.Enum;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/topic")]
public class TopicController(IMediator mediator) : Controller
{

    private readonly IMediator _mediator = mediator;

    [HttpPost("create")]
    public async Task<IActionResult> CreateTopic(CreateTopicCommand Command)
    {
        return Ok(await _mediator.Send(Command));
    }
    
    [HttpPost("update")]
    public async Task<IActionResult> UpdateTopic(UpdateTopicCommand Command)
    {
        return Ok(await _mediator.Send(Command));
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTopic(int id)
    {
        return Ok(await _mediator.Send(new GetTopicByIdQuery(){Id =id}));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllTopics()
    {
        return Ok(await _mediator.Send(new GetAllTopicsQuery() { }));
    }
    
  
    [HttpGet("module/all")]
    public async Task<IActionResult> GetAllTopicsByModule([FromQuery] int moduleId, [FromQuery] TopicStatus? status)
    {
        var query = new GetAllTopicsByModuleIdQuery
        {
            ModuleId = moduleId,
            Status = status
        };
       
        return Ok( await _mediator.Send(query));
    }

}