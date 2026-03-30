namespace Application.Common.Interfaces
{
    public interface IPerformanceService
    {
        public Task<object> RunTestAsync(string sessionId, int iterations);
    }
}
