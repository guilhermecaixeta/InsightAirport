using InsightAirport.Core;
using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InsightAirport.Infra
{
    public class FlightRepository : IFligthReadRepository, IWriteRepository<Flight, int>
    {
        private readonly InsightAirportContext context;

        public FlightRepository(InsightAirportContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Flight> AddAsync(Flight input, CancellationToken cancellationToken)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(Flight));
            }

            await ExecuteTransaction(async () => await context.Flights.AddAsync(input), cancellationToken);

            return input;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Flight> inputs, CancellationToken cancellationToken)
        {
            var flights = inputs.Select(flight => flight);

            await ExecuteTransaction(async () => await context.Flights.AddRangeAsync(flights), cancellationToken);

            return true;
        }

        public Task<bool> AnyAsync(QueryFlight? query, CancellationToken cancellationToken)
        {
            var queryable = GetQueryFilter(query);
            return queryable.AnyAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Flight>> GetAllAsync(QueryFlight? query, CancellationToken cancellationToken)
        {
            var queryable = GetQueryFilter(query);

            return await queryable
                .AsNoTracking()
                .OrderByDescending(x => x.LastUpdate)
                .Select(x => x)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> RemoveAsync(int input, CancellationToken cancellationToken)
        {
            await ExecuteTransaction(async () =>
            {
                var flight = await context.Flights.SingleAsync(x => x.Id == input);
                context.Remove(flight);
            }, cancellationToken);

            return true;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<int> inputs, CancellationToken cancellationToken)
        {
            await ExecuteTransaction(async () =>
            {
                var flights = await context.Flights.Where(x => inputs.Contains(x.Id)).ToListAsync();
                context.RemoveRange(flights);
            }, cancellationToken);

            return true;
        }

        public async Task<Flight> SingleAsync(QueryFlight? query, CancellationToken cancellationToken)
        {
            var queryable = GetQueryFilter(query);

            return await queryable
                .SingleAsync(cancellationToken);
        }

        public async Task<Flight?> SingleOrDefaultAsync(QueryFlight? query, CancellationToken cancellationToken)
        {
            var queryable = GetQueryFilter(query);

            var flight = await queryable
                .SingleOrDefaultAsync(cancellationToken);

            if (flight == null)
            {
                return null;
            }

            return flight;
        }

        public async Task<bool> UpdateAsync(Flight input, CancellationToken cancellationToken)
        {
            await ExecuteTransaction(async () =>
            {
                var flightOutdated = await context.Flights.SingleAsync(f => f.Id == input.Id);

                if (flightOutdated == null)
                {
                    throw new ArgumentNullException(nameof(Flight));
                }

                context.Entry(flightOutdated).CurrentValues.SetValues(input);
            }, cancellationToken);

            return true;
        }

        public Task<bool> CheckStatusWithDifferentFlightId(int id, Status status, CancellationToken cancellationToken)
        {
            return context.Flights.AnyAsync(flight => flight.Id != id && flight.Status == status, cancellationToken);
        }

        private async Task ExecuteTransaction(Func<Task> function, CancellationToken cancellationToken)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            await function();

            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

        private IQueryable<Flight> GetQueryFilter(QueryFlight? query)
        {
            var queryable = context.Flights.AsNoTracking().AsQueryable();

            if (query is not null)
            {
                if (query.Id.HasValue)
                {
                    queryable = queryable.Where(flight => flight.Id == query.Id);
                }

                if (query.Status.HasValue)
                {
                    queryable = queryable.Where(flight => flight.Status == query.Status);
                }

                if (!string.IsNullOrEmpty(query.FlightCode))
                {
                    queryable = queryable.Where(flight => flight.FlightCode.Contains(query.FlightCode, StringComparison.CurrentCultureIgnoreCase));
                }

                if (!string.IsNullOrEmpty(query.Model))
                {
                    queryable = queryable.Where(flight => flight.Model.Contains(query.Model, StringComparison.CurrentCultureIgnoreCase));
                }

                if (query.LastUpdate.HasValue)
                {
                    queryable = queryable.Where(flight => flight.LastUpdate == query.LastUpdate);
                }
            }

            return queryable;
        }
    }
}
