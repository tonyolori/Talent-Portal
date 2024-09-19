using Application.Auth.Commands;
using Application.Students.Queries;

//using Application.AuthController.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("{id}")]
        public async Task<IActionResult> GetStudent(string id)
        {
            return Ok(await _mediator.Send(new GetStudentByIdQuery{Id =id}));
        }
        
           
        [HttpPost("application-history/{studentId}")]
        public async Task<IActionResult> GetStudentApplicationHistory(string  studentId)
        {
            return Ok(await _mediator.Send(new GetStudentApplicationHistoryQuery{StudentId = studentId}));
        }
        
        [HttpPost("programme-type/{studentId}")]
        public async Task<IActionResult> GetStudentPaymentType(string  studentId)
        {
            return Ok(await _mediator.Send(new GetStudentProgrammePaymentTypeByIdQuery{StudentId = studentId}));
        }
        
        [HttpPost("all")]
        public async Task<IActionResult> GetAllStudent()
        {
            return Ok(await _mediator.Send(new GetAllStudentsQuery(){}));
        }

        [HttpPost("update-details")]
        public async Task<IActionResult> UpdateStudent(UpdateStudentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        
        [HttpPost("deactivate")]
        public async Task<IActionResult> DeactivateStudent(DeactivateStudentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

      
    }
}