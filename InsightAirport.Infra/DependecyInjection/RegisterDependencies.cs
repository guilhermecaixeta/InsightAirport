using InsightAirport.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InsightAirport.Infra.DependecyInjection
{
    public static class RegisterDependencies
    {
        public static IServiceCollection AddInfraRepositories(this IServiceCollection services)
        {
            services.Scan(classes =>
                classes
                    .FromAssembliesOf(typeof(IReadRepository<,>), typeof(InsightAirportContext))
                    .AddClasses(filter => filter.Where(c => c.Name.EndsWith("repository", StringComparison.InvariantCultureIgnoreCase)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            return services;
        }
    }
}
