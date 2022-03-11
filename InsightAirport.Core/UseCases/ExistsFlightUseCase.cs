using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;

namespace InsightAirport.Core.UseCases
{

    internal class ExistsFlightUseCase : IUseCase<QueryFlight, bool>
    {
        private readonly IFligthReadRepository _readRepository;

        public string Name => nameof(ExistsFlightUseCase);

        public ExistsFlightUseCase(IFligthReadRepository readRepository)
        {
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public Task<bool> HandleAsync(QueryFlight? query, CancellationToken cancellationToken)
        {
            return _readRepository.AnyAsync(query, cancellationToken);
        }
    }
}
