using Application.Instructors.Commands;
using Application.Instructors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/instructor")]
    [ApiController]
    public class InstructorManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InstructorManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateInstructor( CreateInstructorCommand command)
        {
            return Ok(await _mediator.Send(command));
         
        }

      
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateInstructor( UpdateInstructorCommand command)
        { 
            return Ok( await _mediator.Send(command));
        
        }


        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateInstructor(int id)
        {
    
            return Ok(await _mediator.Send( new DeactivateInstructorCommand { InstructorId = id }));
          
        }

        [HttpPost("reactivate/{id}")]
        public async Task<IActionResult> ReactivateInstructor(int id)
        {
            return Ok(await _mediator.Send( new ReactivateInstructorCommand { InstructorId = id }));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructorById(int id)
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
