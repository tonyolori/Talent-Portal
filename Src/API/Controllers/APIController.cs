using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    public class APIController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private IMediator _mediator;
        private string accessToken = string.Empty;
        //protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public string UserId;

        public APIController(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _mediator = mediator;
            _contextAccessor = contextAccessor;


            var accessToken = _contextAccessor?.HttpContext?.Request.Cookies["talent-portal-accessToken"];
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = tokenHandler.ReadJwtToken(accessToken);
            UserId = securityToken.Claims.First(x => x.Value == "NameIdentifier").ToString();

        }

        protected string GetActorEmail()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
    }
}
