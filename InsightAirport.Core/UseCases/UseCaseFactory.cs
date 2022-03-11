using InsightAirport.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InsightAirport.Core.UseCases
{
    internal class UseCaseFactory : IUseCaseFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UseCaseFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Task<TOut> CreateAsync<TIn, TOut>(string useCaseName, TIn input, CancellationToken cancellationToken)
        {
            var useCase = _serviceProvider
                .GetServices<IUseCase<TIn, TOut>>()
                .SingleOrDefault(service => service.Name.Equals(useCaseName, StringComparison.CurrentCulture)) ??
                throw new NullReferenceException($"Use case '{useCaseName}' for TIn '{typeof(TIn).Name}' and TOut '{typeof(TOut).Name}' was not found");

            return useCase.HandleAsync(input, cancellationToken);
        }
    }
}
