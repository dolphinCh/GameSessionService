using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Session.Query
{
    public record GetSessionQuery(string SessionId) : IRequest<(SessionDto, bool cacheHit)>
    {
    }

    public class GetSessionQueryHandler(IApplicationDbContext applicationDbContext, IMemoryCache cache) : IRequestHandler<GetSessionQuery, (SessionDto, bool cacheHit)>
    {
        private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;
        private readonly IMemoryCache _cache = cache;
        private readonly int _ttl = 60;

        public async Task<(SessionDto, bool cacheHit)> Handle(GetSessionQuery request, CancellationToken cancellationToken)
        {
            //Check if there is a record in the cache with that SessionId
            if (_cache.TryGetValue(request.SessionId, out SessionDto cached))
            {
                //The record is found in the cache return it
                return (cached, true);
            }

            //If the record is not found from cache than fech it from database
            var session = await _applicationDbContext.AsEntity<GameSession>()
            .FirstOrDefaultAsync(u => u.SessionId == request.SessionId, cancellationToken);

            //If the record is not found in cache nor in db return empty object (in real application error message)
            if (session == null) return (null, false);

            //transform the object into dto
            var dto = new SessionDto()
            {
                SessionId = session!.SessionId ?? string.Empty,
                Status = session!.Status ?? string.Empty,
                StartedAt = session!.StartedAt
            };

            //add the dto in cache with 60sec TTL
            _cache.Set(request.SessionId, dto, TimeSpan.FromSeconds(_ttl));

            return (dto, true);
        }
    }
}
