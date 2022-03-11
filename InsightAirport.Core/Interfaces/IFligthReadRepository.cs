using InsightAirport.Core.Entities;

namespace InsightAirport.Core.Interfaces
{
    public interface IFligthReadRepository : IReadRepository<QueryFlight, Flight>
    {
        Task<bool> CheckStatusWithDifferentFlightId(int id, Status status, CancellationToken cancellationToken);
    }
}
