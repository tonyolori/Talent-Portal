using Application.Auth.Commands;
using Application.Students.Queries;

//using Application.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class Student(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("id/{id}")]
        public async Task<IActionResult> GetStudent(string id)
        {
            return Ok(await _mediator.Send(new GetStudentByIdQuery{Id =id}));
        }

      
    }
}