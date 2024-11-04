using Application.Calendar.Commands;
using Application.Calendar.Queries;
using Application.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;


namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/TimeTable")]
    public class TimeTableController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string UserId = string.Empty;
        public TimeTableController(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
            var accessToken = _contextAccessor?.HttpContext?.Request.Cookies["talent-portal-accessToken"];
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = tokenHandler.ReadJwtToken(accessToken);
            UserId = securityToken.Claims.First(x => x.Value == "NameIdentifier").ToString();
        }
        [HttpGet("GetSchedule")]
        public async Task<IActionResult> GetSchedule([FromQuery]GetCourseScheduleQuery query)
        {
            query.UserId = UserId;
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("addCourse")]
        public async Task<IActionResult> CreateTimeTableSlot(AddCourseToCalendarCommand command)
        {

            return Ok(await _mediator.Send(command));
        }

        [HttpPost("deleteCourse")]
        public async Task<IActionResult> DeleteTimeTableSlot(DeleteCourseFromCalendarCommand command)
        {

            return Ok(await _mediator.Send(command));
        }
    }
}
