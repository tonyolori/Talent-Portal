using Application.Students.Commands;
using Application.Users.Commands;
//using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

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
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
