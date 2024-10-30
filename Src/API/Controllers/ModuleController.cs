using Application.Auth.Commands;
using Application.Modules.Commands;
using Application.Modules.Queries;
using Application.Students.Queries;

//using Application.AuthController.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/module")]
    public class ModuleController(IMediator mediator,IHttpContextAccessor contextAccessor) : APIController(mediator, contextAccessor)
    {
        [HttpPost("create-module")]
        public async Task<IActionResult> CreateModule([FromForm]CreateModuleCommand command)
        {

            return Ok(await _mediator.Send(command));
        }
        
      
        [Authorize(Roles = "Admin")]  
        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateModuleStatus( UpdateModuleStatusCommand command)
        {

            return Ok(await _mediator.Send(command));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModuleById(int id)
        {
            return Ok(await _mediator.Send(new GetModuleByIdQuery{Id =id}));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllModule()
        {
            return Ok(await _mediator.Send(new GetAllModulesQuery()));
        }

        [HttpGet("programme/{Id}")]
        public async Task<IActionResult> GetModuleByProgrammeId(int Id)
        {
            return Ok(await _mediator.Send(new GetModulesByProgrammeIdQuery { ProgrammeId = Id }));
        }

        
        [HttpPost("delete/{Id}")]
        public async Task<IActionResult> DeleteModuleById(int Id)
        {
            return Ok(await _mediator.Send(new DeleteModuleByIdQuery { Id = Id }));
        }


    }
}