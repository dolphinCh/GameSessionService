using Application.Common.Interfaces;
using Application.Session.Query;
using MediatR;
using System.Diagnostics;

namespace Application.Common.Helpers.Service.Diagnostic
{
    public class PerformanceService(IMediator mediator) : IPerformanceService
    {
        private readonly IMediator _mediator = mediator;

        public async Task<object> RunTestAsync(string sessionId, int iterations)
        {
            var sw = Stopwatch.StartNew();
            List<TimeByIteration> timeByIterations = [];

            for (int i = 0; i < iterations; i++)
            {
                var swByIteration = Stopwatch.StartNew();
                await _mediator.Send(new GetSessionQuery(sessionId));
                swByIteration.Stop();
                timeByIterations.Add(new TimeByIteration(i,swByIteration.ElapsedMilliseconds));
            }

            sw.Stop();

            return new
            {
                iterations,
                elapsedMs = sw.ElapsedMilliseconds,
                timeByIterations,
                avgMs = sw.ElapsedMilliseconds / (double)iterations
            };
        }
    }

    public record TimeByIteration(int Iteration, long Time);
}
