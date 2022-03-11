namespace InsightAirport.Core.Interfaces
{
    public interface IUseCase<TIn>
    {
        Task HandleAsync(TIn input, CancellationToken cancellationToken);
    }

    public interface IUseCase<TIn, TOut>
    {
        public string Name { get; }

        Task<TOut> HandleAsync(TIn input, CancellationToken cancellationToken);
    }
}
