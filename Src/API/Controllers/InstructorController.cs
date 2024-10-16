using Application.Instructors.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/instructor")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InstructorController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

      
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateInstructor( UpdateInstructorCommand command)
        { 
            return Ok( await _mediator.Send(command));
        
        }


        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateInstructor(string id)
        {
    
            return Ok(await _mediator.Send( new DeactivateInstructorCommand { InstructorId = id }));
          
        }

        [HttpPost("reactivate/{id}")]
        public async Task<IActionResult> ReactivateInstructor(string id)
        {
            return Ok(await _mediator.Send( new ReactivateInstructorCommand { InstructorId = id }));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructorById(string id)
        { 
            return Ok(await _mediator.Send(new GetInstructorByIdQuery { InstructorId = id }));

        }
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllInstructors()
        {

            return Ok(await _mediator.Send(new GetAllInstructorsQuery()));

        }
    }
}
