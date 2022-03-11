namespace InsightAirport.Central
{
    public interface IFlightUpdater
    {
        Task UpdateAsync(CancellationToken cancellationToken);
    }
}
