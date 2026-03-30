using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Session.Command
{

    public record StartSessionCommand() : IRequest<SessionDto>
    {
        public required string PlayerId { get; set; }
        public required string GameId { get; set; }

        public static GameSession MapRequestToGameSession(StartSessionCommand user)
        {
            return new GameSession
            {
                GameId = user.GameId,
                PlayerId = user.PlayerId,
                Status = "Active",
                SessionId = Guid.NewGuid().ToString(),
                StartedAt = DateTime.UtcNow,
            };
        }
    }

    public class StartSessionCommandHandler(IApplicationDbContext applicationDbContext, 
        ILogger<StartSessionCommandHandler> logger) : IRequestHandler<StartSessionCommand, SessionDto>
    {
        private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
        private readonly ILogger<StartSessionCommandHandler> _logger = logger;

        public async Task<SessionDto?> Handle(StartSessionCommand request, CancellationToken cancellationToken)
        {
            var newSession = StartSessionCommand.MapRequestToGameSession(request);

            //add the new object in db
            _applicationDbContext.AsEntity<GameSession>().Add(newSession);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Session started {@Session}", newSession);

            var response = new SessionDto()
            {
                SessionId = newSession.SessionId,
                Status = newSession.Status,
                StartedAt = newSession.StartedAt
            };

            return response;
        }

    }
}
