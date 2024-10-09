using Application.Auth.Commands;  
using MediatR;  
using Microsoft.AspNetCore.Mvc;  

namespace API.Controllers  
{  
    [ApiController]  
    [Route("api/auth")]  
    public class StudentAuthenticationController : ControllerBase 
    {  
        private readonly IMediator _mediator;  

        public StudentAuthenticationController(IMediator mediator, IHttpContextAccessor httpContextAccessor) // Update constructor  
        {  
            _mediator = mediator;  
        }  

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
        public async Task<IActionResult> Login(LoginStudentCommand command)  
        {  

            return Ok(await _mediator.Send(command));  
        }  

        [HttpPost("forgot-password")]  
        public async Task<IActionResult> ForgetPassword(ForgotPasswordCommand command)  
        {  
            return Ok(await _mediator.Send(command));  
        }  
        
        [HttpPost("verify-reset-code")]  
        public async Task<IActionResult> VerifyResetCode(VerifyForgotPasswordCodeCommand command)  
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