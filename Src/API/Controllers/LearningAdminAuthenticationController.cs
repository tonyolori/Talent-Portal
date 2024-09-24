using Application.Admins.Commands;
using Application.LearningAdmins.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class LearningAdminAuthenticationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterLearningAdminCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginLearningAdminCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgetPassword(ForgotLearningAdminPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetLearningAdminPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}