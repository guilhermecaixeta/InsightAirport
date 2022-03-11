namespace InsightAirport.Core.Interfaces
{
    public interface IStrategy<TIn, TOut>
    {
        string Name { get; }

        Task<TOut> ExecuteAsync(TIn input, CancellationToken cancellationToken);
    }
}
