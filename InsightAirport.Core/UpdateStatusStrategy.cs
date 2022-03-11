using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;

namespace InsightAirport.Core
{
    public class UpdateStatusStrategy : StatusStrategy
    {
        private readonly IFligthReadRepository _readRepository;

        public UpdateStatusStrategy(IFligthReadRepository readRepository)
        {
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public override string Name => nameof(UpdateStatusStrategy);

        public override Task<bool> ExecuteAsync(Flight input, CancellationToken cancellationToken)
        {
          return IsValidStatusAsync(input, cancellationToken);
        }

        protected override Task<bool> CheckFlightStatusAsync(Flight input, Status departing, CancellationToken cancellationToken)
        {
            return _readRepository.CheckStatusWithDifferentFlightId(input.Id, departing, cancellationToken);
        }
    }
}
