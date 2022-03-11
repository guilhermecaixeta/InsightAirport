using InsightAirport.Central.Models;
using Refit;
using System.Net;
using System.Text.Json;

namespace InsightAirport.Central
{
    internal class FlightsGenerator : IFlightsGenerator
    {
        const string AllowedLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string AllowedNumbers = "0123456789";

        private readonly InsightAirportClient insightAirportClient;
        private readonly ILogger<FlightsGenerator> _logger;

        private readonly Random random = new Random();

        private readonly string[] models = new[]
        {
            "Airbus A300B1",
            "Airbus A300B2",
            "Airbus A300B4",
            "Airbus A330-800neo",
            "Airbus A380plus",
            "ATR 42-200",
            "ATR 72-210",
            "ATR 72-100",
            "Boeing 737",
            "Boeing 747",
            "Boeing 767-400ER",
            "Boeing 787-10",
            "Cessna 400 Corvalis TT",
            "Cessna XMC",
            "Concorde",
            "Embraer ERJ-135",
            "Embraer 195-E2",
            "Embraer EMB-121 Xingu"
        };

        public FlightsGenerator(InsightAirportClient insightAirportClient, ILogger<FlightsGenerator> logger)
        {
            this.insightAirportClient = insightAirportClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Flight>> GenerateAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Retrieving all flights from api");
            var flights = await SafeRequest(() => insightAirportClient.GetFlights(null));
            
            var totalFlights = flights.Count();
            _logger.LogDebug($"Was found '{totalFlights}' flights");

            if (totalFlights >= 10)
            {
                return flights;
            }

            var flightNumber = random.Next(totalFlights, 20);
            _logger.LogDebug($"Generating flights '{flightNumber}' to complement");

            var missingFlights = GenerateFlights(flightNumber);
            await SavingMissingFlights(missingFlights, cancellationToken);

            return await SafeRequest(() => insightAirportClient.GetFlights(null));
        }

        private async Task<IEnumerable<Flight>> SafeRequest(Func<Task<IEnumerable<Flight>>> request)
        {
            try
            {
                return await request();
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);

                return new List<Flight>();
            }
        }

        private async Task SavingMissingFlights(IEnumerable<Flight> missingFlights, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug($"Saving all flights");
                var tasks = missingFlights.Select(async flight => await insightAirportClient.PostFlight(flight));
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private IEnumerable<Flight> GenerateFlights(int flightNumber)
        {
            for (int i = 0; i < flightNumber; i++)
            {
                var code = GenerateCode();
                Status status = GenerateStatus();
                var model = models[random.Next(models.Length - 1)];
                yield return new Flight(0, code, model, status);
            }
        }

        private Status GenerateStatus() => 
            (Status)random.Next(5, 7);

        private string GenerateCode()
        {
            var stringCode = new string(Enumerable.Repeat(AllowedLetters, 2).Select(s => s[random.Next(s.Length)]).ToArray());
            var stringNumber = new string(Enumerable.Repeat(AllowedNumbers, 3).Select(s => s[random.Next(s.Length)]).ToArray());
            return $"{stringCode}-{stringNumber}";
        }
    }
}
