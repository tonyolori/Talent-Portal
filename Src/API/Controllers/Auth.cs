using Application.Auth.Commands;

//using Application.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class Auth(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterStudentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        
        [HttpPost("confirm-registration")]
        public async Task<IActionResult> ConfirmRegistration(ConfirmRegistrationCommand command)
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
