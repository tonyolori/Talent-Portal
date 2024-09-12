using Application.Tasks.Commands;
using Application.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Notifications.Queries;

namespace API.Controllers;

[ApiController]
[Route("api/Notifications")]
public class NotificationController(IMediator mediator) : Controller { 
    private readonly IMediator _mediator = mediator;


    [HttpGet("getUnread/{studentId}")]
    public async Task<IActionResult> GetUnreadNotifications(string studentId)
    {
        return Ok(await _mediator.Send(new GetUnreadNotificationsQuery { StudentId = studentId }));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllNotificationsQuery { }));
    }

}
