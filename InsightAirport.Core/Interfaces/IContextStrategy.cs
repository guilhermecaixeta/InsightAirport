namespace InsightAirport.Core.Interfaces
{
    public interface IContextStrategy
    {
        Task<TOut> HandleAsync<TIn, TOut>(string strategyName, TIn input, CancellationToken cancellationToken);
    }
}
