using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;

namespace InsightAirport.Core.UseCases
{
    internal class GetFlightUseCase : IUseCase<int, Flight?>
    {
        private readonly IFligthReadRepository _readRepository;

        public GetFlightUseCase(IFligthReadRepository readRepository)
        {
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public string Name => nameof(GetFlightUseCase);

        public Task<Flight?> HandleAsync(int input, CancellationToken cancellationToken)
        {
            var query = new QueryFlight { Id = input };

            return _readRepository.SingleOrDefaultAsync(query, cancellationToken);
        }
    }
}
