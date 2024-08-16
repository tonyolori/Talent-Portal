using Application.Students.Commands;
using Application.Users.Commands;
//using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterStudentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
       
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        
    }
}
