using InsightAirport.Central.Models;

namespace InsightAirport.Central
{
    public interface IFlightsGenerator
    {
        Task<IEnumerable<Flight>> GenerateAsync(CancellationToken cancellationToken);
    }
}
