using InsightAirport.Central;
using Refit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .AddRefitClient<InsightAirportClient>()
            .ConfigureHttpClient(config =>
            {
                config.BaseAddress = new Uri("http://insightairport:80/api");
                config.Timeout = TimeSpan.FromSeconds(90);
            });

        services.AddScoped<IFlightsGenerator, FlightsGenerator>();
        services.AddScoped<IFlightUpdater, FlightUpdater>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
