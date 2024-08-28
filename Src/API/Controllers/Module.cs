using Application.Auth.Commands;
using Application.Modules.Commands;
using Application.Students.Queries;

//using Application.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/module")]
    public class Module(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("create-module")]
        public async Task<IActionResult> CreateModule(CreateModuleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

      
    }
}