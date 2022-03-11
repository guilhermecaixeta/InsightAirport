namespace InsightAirport.Core.Interfaces
{
    public interface IUseCaseFactory
    {
        Task<TOut> CreateAsync<TIn, TOut>(string useCaseName, TIn input, CancellationToken cancellationToken);
    }
}
