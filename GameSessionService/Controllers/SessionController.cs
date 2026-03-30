using Application.Session.Command;
using Application.Session.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GameSessionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionController(ISender sender, IHttpContextAccessor _httpContextAccessor) : ControllerBase
    {
        [HttpGet("{sessionId}")]
        public async Task<IResult> GetAsync(string sessionId)
        {
            var query = new GetSessionQuery(sessionId);
            var (session, cacheHit) = await sender.Send(query);
            _httpContextAccessor.HttpContext!.Response.Headers["X-Cache"] = cacheHit ? "Hit" : "Miss";
            return session is null ? Results.NotFound() : Results.Ok(session);
        }

        [HttpPost]
        public async Task<IResult> StartSessionAsync(StartSessionCommand command)
        {
            var session = await sender.Send(command);
            return Results.Ok(session);
        }
    }
}
