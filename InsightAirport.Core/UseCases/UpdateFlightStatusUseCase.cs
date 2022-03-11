using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;

namespace InsightAirport.Core.UseCases
{
    public class UpdateFlightStatusUseCase : IUseCase<int, bool>
    {
        private readonly IFligthReadRepository _readRepository;
        private readonly IWriteRepository<Flight, int> _writeRepository;
        private readonly IContextStrategy _contextStrategy;

        public UpdateFlightStatusUseCase(
            IFligthReadRepository readRepository,
            IWriteRepository<Flight, int> writeRepository,
            IContextStrategy contextStrategy)
        {
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
            _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
            _contextStrategy = contextStrategy ?? throw new ArgumentNullException(nameof(contextStrategy));
        }

        public string Name => nameof(UpdateFlightStatusUseCase);

        public async Task<bool> HandleAsync(int input, CancellationToken cancellationToken)
        {
            var query = new QueryFlight { Id = input };

            var flight = await _readRepository.SingleAsync(query, cancellationToken);

            flight.ChangeStatus();

            var hasFlightWithStatus = await _contextStrategy.HandleAsync<Flight, bool>(nameof(UpdateStatusStrategy), flight, cancellationToken);

            if (hasFlightWithStatus)
            {
                throw new Exception($"Already exists an airplane in status '{flight.Status}'");
            }

            return await _writeRepository.UpdateAsync(flight, cancellationToken);
        }
    }
}
