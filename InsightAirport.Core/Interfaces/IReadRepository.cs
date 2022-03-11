namespace InsightAirport.Core.Interfaces
{
    public interface IReadRepository<TIn, TOut>
    {
        Task<bool> AnyAsync(TIn? query, CancellationToken cancellationToken);

        Task<TOut> SingleAsync(TIn? query, CancellationToken cancellationToken);

        Task<TOut?> SingleOrDefaultAsync(TIn? query, CancellationToken cancellationToken);

        Task<IReadOnlyList<TOut>> GetAllAsync(TIn? query, CancellationToken cancellationToken);
    }
}
