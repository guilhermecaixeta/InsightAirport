using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;

namespace InsightAirport.Core
{
    public class AddStatusStrategy : StatusStrategy
    {
        private readonly IFligthReadRepository _readRepository;

        public AddStatusStrategy(IFligthReadRepository readRepository)
        {
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public override string Name => nameof(AddStatusStrategy);

        public override Task<bool> ExecuteAsync(Flight input, CancellationToken cancellationToken)
        {
          return IsValidStatusAsync(input, cancellationToken);
        }

        protected override Task<bool> CheckFlightStatusAsync(Flight input, Status status, CancellationToken cancellationToken) =>
            _readRepository.AnyAsync(new QueryFlight { Status = status }, cancellationToken);
    }
}
