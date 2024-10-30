
using Application.Interfaces;
using Application.Paystack.Commands;
using Application.Paystack.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/payment")]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IGenerateToken _generateToken;

    public PaymentController(IMediator mediator, IGenerateToken generateToken)
    {
        _mediator = mediator;
        _generateToken = generateToken;
    }

    // [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment(CreatePaymentCommand command)
    {
        string email = _generateToken.GetEmailFromToken(User);
        command.Email = email;
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyPayment(VerifyPaymentCommand command)
    {

        return Ok(await _mediator.Send(command));
    }
    
    
    [HttpPost("payment-history/{studentId}")]
    public async Task<IActionResult> GetStudentApplicationHistory(string  studentId)
    {
        return Ok(await _mediator.Send(new GetStudentPaymentHistoryQuery(){StudentId = studentId}));
    }
        
    [HttpPost("payment-type/{studentId}")]
    public async Task<IActionResult> GetStudentPaymentType(string  studentId)
    {
        return Ok(await _mediator.Send(new GetStudentPaymentTypeByIdQuery{StudentId = studentId}));
    }
}
