using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;

namespace InsightAirport.Core
{
    public abstract class StatusStrategy : IStrategy<Flight, bool>
    {
        public abstract string Name { get; }

        public abstract Task<bool> ExecuteAsync(Flight input, CancellationToken cancellationToken);

        protected async Task<bool> IsValidStatusAsync(Flight input, CancellationToken cancellationToken)
        {
            switch (input.Status)
            {
                case Status.Departing:
                    return await CheckFlightStatusAsync(input, Status.Departing, cancellationToken);
                case Status.Landing:
                    return await CheckFlightStatusAsync(input, Status.Landing, cancellationToken);
                default:
                    return false;
            };
        }

        protected abstract Task<bool> CheckFlightStatusAsync(Flight input, Status departing, CancellationToken cancellationToken);
    }
}
