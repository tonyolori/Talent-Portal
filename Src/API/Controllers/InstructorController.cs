using Application.Instructors.Commands;
using Application.Instructors.Queries;
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
        
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateInstructor([FromBody] CreateInstructorCommand command)
        {
            return Ok(await _mediator.Send(command));
         
        }

      
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateInstructor(int id, [FromBody] UpdateInstructorCommand command)
        { 
            return Ok( await _mediator.Send(command));
        
        }


        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateInstructor(int id)
        {
    
            return Ok(new DeactivateInstructorCommand { InstructorId = id });
          
        }

        [HttpPost("reactivate/{id}")]
        public async Task<IActionResult> ReactivateInstructor(int id)
        {
            return Ok( new ReactivateInstructorCommand { InstructorId = id });
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructorById(int id)
        { 
            return Ok(new GetInstructorByIdQuery { InstructorId = id });

        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllInstructors()
        {

            return Ok(await _mediator.Send(new GetAllInstructorsQuery()));

        }
    }
}
