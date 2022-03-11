using InsightAirport.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InsightAirport.Core
{
    public class ContextStrategy : IContextStrategy
    {
        private readonly IServiceProvider _serviceProvider;

        public ContextStrategy(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Task<TOut> HandleAsync<TIn, TOut>(string strategyName, TIn input, CancellationToken cancellationToken)
        {
            var strategy = _serviceProvider
                .GetServices<IStrategy<TIn, TOut>>()
                .SingleOrDefault(strategy => strategy.Name.Equals(strategyName, StringComparison.InvariantCultureIgnoreCase))
                ?? throw new NullReferenceException($"Strategy {strategyName} not found");

            return strategy.ExecuteAsync(input, cancellationToken);
        }
    }
}
