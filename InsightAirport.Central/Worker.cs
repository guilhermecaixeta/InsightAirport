namespace InsightAirport.Central
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IFlightsGenerator flightsGenerator;
        private readonly IFlightUpdater flightUpdater;

        public Worker(ILogger<Worker> logger, IFlightsGenerator flightsGenerator, IFlightUpdater flightUpdater)
        {
            _logger = logger;
            this.flightsGenerator = flightsGenerator;
            this.flightUpdater = flightUpdater;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    //await flightsGenerator.GenerateAsync(stoppingToken);
                    //await flightUpdater.UpdateAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                Dispose();
            }
        }


    }
}