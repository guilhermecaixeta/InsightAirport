using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;

namespace InsightAirport.Core.UseCases
{
    internal class GetFlightsUseCase : IUseCase<QueryFlight, IReadOnlyList<Flight>>
    {
        private readonly IFligthReadRepository _readRepository;

        public GetFlightsUseCase(IFligthReadRepository readRepository)
        {
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public string Name => nameof(GetFlightsUseCase);

        public Task<IReadOnlyList<Flight>> HandleAsync(QueryFlight query, CancellationToken cancellationToken)
        {
            return _readRepository.GetAllAsync(query, cancellationToken);
        }
    }
}
