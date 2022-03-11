using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InsightAirport.Infra
{
    public static class Migrations
    {
        public static void RunMigrations(this IServiceCollection serviceCollection)
        {
            var dbContext = serviceCollection.BuildServiceProvider().GetService<InsightAirportContext>()!;

            dbContext.Database.Migrate();
        }
    }
}
