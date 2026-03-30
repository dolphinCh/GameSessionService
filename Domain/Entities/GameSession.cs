using Domain.Common;

namespace Domain.Entities
{
    public class GameSession : BaseEntity
    {
        public required string SessionId { get; set; }
        public required string PlayerId { get; set; }
        public required string GameId { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public required string Status { get; set; }
    }
}
