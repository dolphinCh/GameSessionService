using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameSessionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiagnosticsController(IPerformanceService performanceService) : ControllerBase
    {
        [HttpGet("perf-test")]
        public async Task<IResult> GetAsync([FromQuery] string sessionId, [FromQuery] int iterations)
        {
            var result = await performanceService.RunTestAsync(sessionId, iterations);
            return Results.Ok(result);
        }
    }
}
