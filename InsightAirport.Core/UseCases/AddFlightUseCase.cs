using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;

namespace InsightAirport.Core.UseCases
{
    public class AddFlightUseCase : IUseCase<Flight, Flight>
    {
        private readonly IWriteRepository<Flight, int> _writeRepository;
        private readonly IContextStrategy _contextStrategy;

        public AddFlightUseCase(IWriteRepository<Flight, int> writeRepository, IContextStrategy contextStrategy)
        {
            _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
            _contextStrategy = contextStrategy ?? throw new ArgumentNullException(nameof(contextStrategy));
        }

        public string Name => nameof(AddFlightUseCase);

        public async Task<Flight> HandleAsync(Flight input, CancellationToken cancellationToken)
        {
            var hasFlightWithStatus = await _contextStrategy.HandleAsync<Flight, bool>(nameof(AddStatusStrategy), input, cancellationToken);

            if (hasFlightWithStatus)
            {
                throw new Exception($"Already exists an airplane in status '{input.Status}'");
            }

            return await _writeRepository.AddAsync(input, cancellationToken);
        }
    }
}
