namespace InsightAirport.Core.Interfaces
{
    public interface IWriteRepository<TObj, TKey> : IAdapter
    {
        public Task<TObj> AddAsync(TObj input, CancellationToken cancellationToken);
        public Task<bool> AddRangeAsync(IEnumerable<TObj> inputs, CancellationToken cancellationToken);
        public Task<bool> UpdateAsync(TObj input, CancellationToken cancellationToken);
        public Task<bool> RemoveAsync(TKey input, CancellationToken cancellationToken);
        public Task<bool> RemoveRangeAsync(IEnumerable<TKey> inputs, CancellationToken cancellationToken);
    }
}
