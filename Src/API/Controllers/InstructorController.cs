using Application.Instructors.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/instructor")]
    public class InstructorController : APIController
    {
        public InstructorController(IMediator mediator, IHttpContextAccessor contextAccessor)
            : base(mediator, contextAccessor) { }
      
        [Authorize(Roles = "Instructor")]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateInstructor( UpdateInstructorCommand command)
        { 
            command.InstructorId = UserId;
            return Ok( await _mediator.Send(command));
        
        }


        [Authorize(Roles = "Admin")]  
        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateInstructor(string id)
        {
    
            return Ok(await _mediator.Send( new DeactivateInstructorCommand { InstructorId = id }));
          
        }

        [Authorize(Roles = "Admin")]  
        [HttpPost("reactivate/{id}")]
        public async Task<IActionResult> ReactivateInstructor(string id)
        {
            return Ok(await _mediator.Send( new ReactivateInstructorCommand() { InstructorId = id }));
        }
        
        [Authorize(Roles = "Instructor")]  
        [HttpGet]  
        public async Task<IActionResult> GetInstructorById()
        {
            var query = new GetInstructorByIdQuery();
            query.InstructorId = UserId;
            return Ok(await _mediator.Send(query));

        }
        
        
        [Authorize(Roles = "Admin")]  
        [HttpGet("all")]
        public async Task<IActionResult> GetAllInstructors()
        {

            return Ok(await _mediator.Send(new GetAllInstructorsQuery()));

        }
    }
}
