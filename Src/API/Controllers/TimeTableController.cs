using Application.Calendar.Commands;
using Application.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/TimeTable")]
    public class TimeTableController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("/addCourse")]
        public async Task<IActionResult> createTimeTable(AddCourseToCalendarCommand command)
        {

            return Ok(await _mediator.Send(command));
        }
    }
}
