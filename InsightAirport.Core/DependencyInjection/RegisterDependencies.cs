using InsightAirport.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InsightAirport.Core.DependencyInjection
{
    public static class RegisterDependencies
    {
        private static readonly string[] dependencies = new[] { "usecase", "factory", "strategy" };

        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.Scan(classes => classes
                .FromAssembliesOf(typeof(IAdapter))
                .AddClasses(filter => 
                    filter.Where(x => dependencies.Any(dependency => x.Name.EndsWith(dependency, StringComparison.InvariantCultureIgnoreCase))))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            return services;
        }
    }
}
